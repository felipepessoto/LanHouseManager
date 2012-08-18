using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LanManager.DAL;

namespace LanManager.BLL
{
    public class ClientSessionManager : BaseManager
    {
        public Client LogOn(string userName, string password, Guid accessPointId)
        {
            //Seleciona o cliente
            var client = (from cl in db.Client.Include("Parent")
                          where cl.UserName == userName && cl.Password == password
                          select cl).FirstOrDefault();

            //Se não encontrou cliente com o nome de usuario e senha
            if (client == null)
            {
                throw new InvalidLogOnException(ResourceSet.Resources.InvalidLogOn);
            }

            //Se tiver sessão aberta
            ClientSession sessaoAberta = CheckOpenSession(client);
            if (sessaoAberta != null)
            {
                //Se a sessao estava aberta neste computador, restaura a sessao
                if (sessaoAberta.AccessPoint.Id == accessPointId)
                {
                    SessionStatus.LoggedClient = client;
                    SessionStatus.CurrentSession = sessaoAberta;

                    return client;
                }
                //Senão gera um erro
                throw new ClientAlreadyLoggedException(ResourceSet.Resources.ClientAlreadyLogged);
            }

            //Se não está ativo
            if (!client.Active)
            {
                throw new ClientInactiveException(ResourceSet.Resources.ClientInactive);
            }

            //Se está desatualizado
            if ((client.LastUpdateDate ?? client.RegisterDate) < DateTime.Now.AddMonths(-3))
            {
                throw new ClientOutDatedException(ResourceSet.Resources.ClientOutDated);
            }

            //Se o cliente não tem mais crédito disponível
            if (client.MinutesLeft + client.MinutesBonus + client.MaxDebit <= 0)
            {
                throw new ClientLowCreditException(ResourceSet.Resources.ClientCreditLimit);
            }

            //Se não for maior de idade
            if (client.BirthDate >= DateTime.Now.AddYears(-18))
            {
                PermissionManager permission = new PermissionManager();
                permission.ValidatePermissionThisHour(client);
            }

            OpenNewSession(client, new AccessPointManager(db).GetAccessPointById(accessPointId));

            return client;
        }

        private ClientSession CheckOpenSession(LanManager.DAL.Client client)
        {

            var session = (from ss in db.ClientSession.Include("AccessPoint")
                           where ss.Client.Id == client.Id && ss.EndDate == null
                           select ss).FirstOrDefault();

            return session;

        }

        private void OpenNewSession(Client client, AccessPoint accessPoint)
        {
            ClientSession session = ClientSession.CreateClientSession(Guid.NewGuid(), DateTime.Now, DateTime.Now);
            session.Client = client;
            session.AccessPoint = accessPoint;

            db.AddToClientSession(session);
            db.SaveChanges();

            SessionStatus.LoggedClient = client;
            SessionStatus.CurrentSession = session;
        }

        public bool CheckSessionIsClosed()
        {
            var currentSession = (from ss in db.ClientSession
                                  where ss.Id == SessionStatus.CurrentSession.Id
                                  select ss).First();

            currentSession.LastClientPing = DateTime.Now;
            db.SaveChanges();

            return currentSession.EndDate.HasValue;
        }

        public static bool CanContinueAfterMidnight()
        {
            return SessionStatus.LoggedClient.BirthDate < DateTime.Now.AddYears(-18) ||
                   SessionStatus.LoggedClient.HasMidnightPermission;
        }

        public bool CheckCantContinueLogged(TimeSpan offset)
        {
            DateTime offsetHour = DateTime.Now.Add(offset);

            //Se o menor de idade está logado a 3 horas ou mais
            if (SessionStatus.LoggedClient.BirthDate >= DateTime.Now.AddYears(-18) &&
                SessionStatus.CurrentSession.StartDate < DateTime.Now.AddHours(-3).Add(offset))
            {
                return true;
            }
            //Se for menor de idade, e não tem permissão pra ficar após meia-noite
            if (SessionStatus.LoggedClient.BirthDate >= DateTime.Now.AddYears(-18) && !SessionStatus.LoggedClient.HasMidnightPermission && offsetHour.Hour < 6)
            {
                return true;
            }

            //Se for menor de idade e não tiver permissao pra acesar qualquer horario, verifica se esta em periodo permitido
            if (SessionStatus.LoggedClient.BirthDate >= DateTime.Now.AddYears(-18) && !SessionStatus.LoggedClient.CanAccessAnyTime && SessionStatus.LoggedClient.Parent != null)
            {
                int diaSemana = (int)offsetHour.DayOfWeek;

                bool permissaoResposavel = (from pr in db.PeriodAllowed
                                            where (pr.DayOfWeek == diaSemana || pr.DayOfWeek == null)
                                                  && pr.StartHour <= offsetHour.Hour
                                                  && pr.EndHour > offsetHour.Hour
                                                  && pr.Client.Id == SessionStatus.LoggedClient.Id
                                            select pr).Any();

                return !permissaoResposavel;
            }

            return false;
        }

        public TimeSpan GetElapsedTime()
        {
            return DateTime.Now - SessionStatus.CurrentSession.StartDate;
        }

        /// <summary>
        /// Close all sessions of logged client
        /// </summary>
        /// <returns></returns>
        public void CloseAllSessions()
        {
            var sessions = (from ss in db.ClientSession
                            where ss.Client.Id == SessionStatus.LoggedClient.Id && ss.EndDate == null
                            select ss.Id).ToList();

            foreach (var session in sessions)
            {
                CloseSession(session, false);
            }
        }

        /// <summary>
        /// Close a specific session
        /// </summary>
        /// <param name="id">Session ID</param>
        public void CloseSession(Guid id, bool isFreeSession)
        {
            var session = (from ss in db.ClientSession.Include("Client")
                           where ss.Id == id
                           select ss).First();

            session.EndDate = DateTime.Now;

            var clientApplicationLog = from appLog in db.ClientApplicationLog
                                       where appLog.Client.Id == session.Client.Id && appLog.EndDate == null
                                       select appLog;

            foreach (var appLog in clientApplicationLog)
            {
                appLog.EndDate = session.EndDate;
            }

            TimeSpan tempoUso = session.EndDate.Value - session.StartDate;

            if (!isFreeSession)
            {
                if (session.Client.MinutesBonus >= (int)tempoUso.TotalMinutes)
                {
                    session.Client.MinutesBonus = session.Client.MinutesBonus - (int)tempoUso.TotalMinutes;
                    session.MinutesBonusUsed = (int)tempoUso.TotalMinutes;
                    session.MinutesPaid = 0;
                }
                else
                {
                    session.MinutesBonusUsed = session.Client.MinutesBonus;
                    session.MinutesPaid = ((int)tempoUso.TotalMinutes - session.Client.MinutesBonus);
                    session.Client.MinutesLeft = session.Client.MinutesLeft - ((int)tempoUso.TotalMinutes - session.Client.MinutesBonus);
                    session.Client.MinutesBonus = 0;
                }
            }

            db.SaveChanges();
        }

        public int GetMinutesLeftWithDebitLimit()
        {
            return SessionStatus.LoggedClient.MinutesBonus + SessionStatus.LoggedClient.MinutesLeft +
                   SessionStatus.LoggedClient.MaxDebit;
        }

        public List<ClientSession> GetOpenSessions(string[] relacionamentosIncluidos)
        {
            IQueryable<ClientSession> query = IncluirRelacionamentos(db.ClientSession, relacionamentosIncluidos);

            var session = (from ss in query
                           where ss.EndDate == null
                           select ss).ToList();

            return session;
        }

        public List<ClientSessionClient> GenerateReport(DateTime? start, DateTime? end, string userName, string[] relacionamentosIncluidos)
        {
            IQueryable<ClientSession> query = IncluirRelacionamentos(db.ClientSession, relacionamentosIncluidos);

            var session = (from ss in query
                           where
                               (ss.StartDate >= start || !start.HasValue) && (ss.EndDate <= end || !end.HasValue) &&
                               (ss.Client.UserName == userName || string.IsNullOrEmpty(userName))
                           orderby ss.StartDate
                           select new ClientSessionClient {Session = ss, Client = ss.Client}).ToList();

            return session;
        }
    }

    public class ClientSessionClient
    {
        public ClientSession Session { get; set; }
        public Client Client { get; set; }
        public TimeSpan Duration
        {
            get
            {
                if (Session.EndDate.HasValue)
                {
                    return Session.EndDate.Value - Session.StartDate;
                }
                return TimeSpan.Zero;
            }
            set { }
        }
    }
}