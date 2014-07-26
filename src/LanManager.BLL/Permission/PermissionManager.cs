using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LanManager.BLL
{
    public class PermissionManager : BaseManager
    {
        public void ValidatePermissionThisHour(Client client)
        {
            //Verifica se tem permissao do responsavel ou não tem responsavel
            if (!client.CanAccessAnyTime && client.Parent != null)
            {
                int diaSemana = (int)DateTime.Now.DayOfWeek;

                bool permissaoResposavel = (from pr in db.PeriodAllowed
                                            where (pr.DayOfWeek == diaSemana || pr.DayOfWeek == null)
                                            && pr.StartHour <= DateTime.Now.Hour
                                            && pr.EndHour > DateTime.Now.Hour
                                            && pr.Client.Id == client.Id
                                            select pr).Any();

                //Se não tem permissao esta hora
                if (!permissaoResposavel)
                {
                    throw new ClientHourPermissionException(ResourceSet.Resources.ClientHourPermission);
                }
            }

            //Se logado em menos de 30 minutos (Lei Estadual SP)
            var dataUltimoLogon = (from ss in db.ClientSession
                                   where ss.Client.Id == client.Id
                                   orderby ss.EndDate descending
                                   select ss.EndDate).FirstOrDefault();

            if (dataUltimoLogon != null && dataUltimoLogon >= DateTime.Now.AddMinutes(-30))
            {
                throw new ClientHourPermissionException(ResourceSet.Resources.Client30MinutesPermission);
            }

            //Se for entre 00:00 e 6 horas o menor de idade deve ter permissao por escrito
            if (DateTime.Now.Hour < 6 && !client.HasMidnightPermission)
            {
                throw new ClientHourPermissionException(ResourceSet.Resources.ClientMidnightPermission);
            }
        }

        public IEnumerable<Application> ReturnApplicationsAllowed()
        {
            int idade = AgeCalc(SessionStatus.LoggedClient.BirthDate);

            if (SessionStatus.LoggedClient.CanAccessAnyApplication || SessionStatus.LoggedClient.BirthDate < DateTime.Now.AddYears(-18))
            {
                return (from app in db.Application.Include("ApplicationGroup")
                        where app.MinimumAge <= idade && app.Active
                        select app).ToList().Where(x => System.IO.File.Exists(x.DefaultPath)).ToList();
            }

            var appAllowed = (from cliente in db.Client.Include("ApplicationsAllowed").Include("ApplicationsAllowed.ApplicationGroup")
                              where cliente.Id == SessionStatus.LoggedClient.Id
                              select cliente).First().ApplicationsAllowed;

            var groupAppAllowed = (from cliente in db.Client.Include("ApplicationGroupsAllowed").Include("ApplicationGroupsAllowed.Application")
                                   where cliente.Id == SessionStatus.LoggedClient.Id
                                   select cliente).First().ApplicationGroupsAllowed;



            return appAllowed.ToList().Union(groupAppAllowed.SelectMany(x => x.Application)).Where(x => System.IO.File.Exists(x.DefaultPath) && x.MinimumAge <= idade && x.Active);
        }

        public void RemovePeriodAllowed(Guid id)
        {
            var appAllowed = (from per in db.PeriodAllowed
                              where per.Id == id
                              select per).First();

            db.DeleteObject(appAllowed);
            db.SaveChanges();
        }

        private static int AgeCalc(DateTime dNasc)
        {
            int idade = DateTime.Now.Year - dNasc.Year;
            if (DateTime.Now.Month < dNasc.Month ||
            (DateTime.Now.Month == dNasc.Month &&
            DateTime.Now.Day < dNasc.Day))
                idade--;
            return idade;
        }
    }
}