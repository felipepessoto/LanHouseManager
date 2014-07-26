using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LanManager.BLL
{
    public class ClientManager : BaseManager
    {
        public void CreateClient(Client newClient)
        {
            db.AddToClient(newClient);
            db.SaveChanges();
        }

        public void CreateMinutesPurchasedHistory(MinutesPurchased minutesPurchased)
        {
            db.AddToMinutesPurchased(minutesPurchased);
        }

        public void DisableClient(Guid clientID)
        {
            var client = (from cl in db.Client
                          where cl.Id == clientID
                          select cl).First();

            client.Active = false;
            db.SaveChanges();
        }

        public void EnableClient(Guid clientID)
        {
            var client = (from cl in db.Client
                          where cl.Id == clientID
                          select cl).First();

            client.Active = true;
            db.SaveChanges();
        }

        public void ChangeCredit(Guid clientID, int value, bool isBonusCredit)
        {

            var client = (from cl in db.Client
                          where cl.Id == clientID
                          select cl).First();
            if (isBonusCredit)
            {
                client.MinutesBonus += value;
            }
            else
            {
                client.MinutesLeft += value;
            }
            db.SaveChanges();
        }

        public List<Client> GetClients(string[] includeRelationships)
        {
            IQueryable<Client> query = IncluirRelacionamentos(db.Client, includeRelationships);

            var clients = (from cl in query
                          select cl).ToList();

            return clients;
        }

        public Client GetClientsById(Guid clientId, string[] includeRelationships)
        {
            IQueryable<Client> query = IncluirRelacionamentos(db.Client, includeRelationships);

            var client = (from cl in query
                           where cl.Id == clientId
                           select cl).First();

            return client;
        }

        public Client SearchByUsername(string username, string[] includeRelationships)
        {
            IQueryable<Client> query = IncluirRelacionamentos(db.Client, includeRelationships);

            var client = (from cl in query
                          where cl.UserName == username
                          select cl).FirstOrDefault();

            return client;
        }

        public Client SearchByDocumentID(string documentID, string[] includeRelationships)
        {
            IQueryable<Client> query = IncluirRelacionamentos(db.Client, includeRelationships);

            var client = (from cl in query
                          where cl.DocumentID == documentID
                          select cl).FirstOrDefault();

            return client;
        }

        public Client SearchByCPF(string cpf, string[] includeRelationships)
        {
            IQueryable<Client> query = IncluirRelacionamentos(db.Client, includeRelationships);

            var client = (from cl in query
                          where cl.CPF == cpf
                          select cl).FirstOrDefault();

            return client;
        }

        public List<Client> GetClients18YearsOld(string[] includeRelationships)
        {
            IQueryable<Client> query = IncluirRelacionamentos(db.Client, includeRelationships);

            DateTime idade = DateTime.Now.AddYears(-18);

            var clients = (from cl in query
                           where cl.BirthDate <= idade
                           select cl).ToList();

            return clients;
        }

        public List<Client> SearchClients(string keyword, string[] includeRelationships)
        {
            IQueryable<Client> query = IncluirRelacionamentos(db.Client, includeRelationships);

            var clients = (from cl in query
                           where string.IsNullOrEmpty(keyword) || cl.UserName.Contains(keyword) || cl.FullName.Contains(keyword)
                           select cl).ToList();

            return clients;
        }

        public List<ReportNewuser> GenerateNewUsersReport(DateTime start, DateTime end, string[] relacionamentosIncluidos)
        {
            IQueryable<Client> query = IncluirRelacionamentos(db.Client, relacionamentosIncluidos);

            var newUsersByDate = (from cl in query
                                 where cl.RegisterDate >= start && cl.RegisterDate <= end
                                 select cl.RegisterDate).ToList();

            var report = (from dates in newUsersByDate
                          group dates by new DateTime(dates.Year, dates.Month, 1)
                          into g
                              select new ReportNewuser() { RegisterDate = g.Key, Count = g.Count() }).ToList();

            return report;
        }

        public void ChangePassword(Client client, string password)
        {
            var selectedClient = (from cl in db.Client
                                  where cl.Id == client.Id
                                  select cl).First();

            selectedClient.Password = password;
            selectedClient.PasswordExpired = false;
            SaveChanges();
        }
    }

    public class ReportNewuser
    {
        public DateTime RegisterDate { get; set; }
        public int Count{ get; set;}
    }
}