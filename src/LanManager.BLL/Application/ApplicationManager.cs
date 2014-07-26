using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LanManager.BLL
{
    public class ApplicationManager : BaseManager
    {
        public ApplicationManager()
        {}

        public ApplicationManager(LanManagerEntities dbEntities)
            : base(dbEntities)
        {}

        public void CreateApplication(Application newApp)
        {
            newApp.Id = Guid.NewGuid();
            db.AddToApplication(newApp);
            db.SaveChanges();
        }

        public void CreateApplicationGroup(ApplicationGroup newGroup)
        {
            newGroup.Id = Guid.NewGuid();
            db.AddToApplicationGroup(newGroup);
            db.SaveChanges();
        }

        public ApplicationGroup SearchGroupById(Guid id, string[] includeRelationships)
        {
            IQueryable<ApplicationGroup> query = IncluirRelacionamentos(db.ApplicationGroup, includeRelationships);

            var group = (from gp in query
                         where gp.Id == id
                         select gp).FirstOrDefault();

            return group;
        }

        public IList<ApplicationGroup> SearchGroupByName(string name, string[] includeRelationships)
        {
            IQueryable<ApplicationGroup> query = IncluirRelacionamentos(db.ApplicationGroup, includeRelationships);

            var group = (from gp in query
                         where gp.Name == name
                         select gp).ToList();

            return group;
        }

        public Application GetApplicationById(Guid id, string[] includeRelationships)
        {
            IQueryable<Application> query = IncluirRelacionamentos(db.Application, includeRelationships);

            var appSelected = (from app in query
                         where app.Id == id
                         select app).FirstOrDefault();

            return appSelected;
        }

        public List<Application> SearchApplications(string keyword, string[] includeRelationships)
        {
            IQueryable<Application> query = IncluirRelacionamentos(db.Application, includeRelationships);

            var apps = (from app in query
                        where string.IsNullOrEmpty(keyword) || app.Name.Contains(keyword) || app.DefaultPath.Contains(keyword)
                        select app).ToList();

            return apps;
        }

        public List<ApplicationGroup> SearchAllGroups(string[] includeRelationships)
        {
            IQueryable<ApplicationGroup> query = IncluirRelacionamentos(db.ApplicationGroup, includeRelationships);

            var groups = (from gp in query
                         select gp).ToList();

            return groups;
        }

        public List<Application> SearchAllApplications(string[] includeRelationships)
        {
            IQueryable<Application> query = IncluirRelacionamentos(db.Application, includeRelationships);

            var apps = (from gp in query
                          select gp).ToList();

            return apps;
        }

        public KeyValuePair<Process, ClientApplicationLog> StartProcess(Application application)
        {
            Process novo = new Process();
            novo.StartInfo.WorkingDirectory = application.DefaultPath.Substring(0, application.DefaultPath.LastIndexOf(System.IO.Path.DirectorySeparatorChar));
            novo.StartInfo.FileName = application.DefaultPath;
            novo.StartInfo.Arguments = application.RunArguments;
            novo.Start();
            return new KeyValuePair<Process, ClientApplicationLog>(novo, WriteClientUseApplicationLog(application));
        }

        private ClientApplicationLog WriteClientUseApplicationLog(Application application)
        {
            ClientApplicationLog clientApplicationLog =
                ClientApplicationLog.CreateClientApplicationLog(Guid.NewGuid(), DateTime.Now);
            clientApplicationLog.ApplicationReference.EntityKey = new EntityKey("LanManagerEntities.Application", "Id",
                                                                                application.Id);
            clientApplicationLog.ClientReference.EntityKey = new EntityKey("LanManagerEntities.Client", "Id",
                                                                           SessionStatus.LoggedClient.Id);
            clientApplicationLog.ClientSessionReference.EntityKey = new EntityKey("LanManagerEntities.ClientSession",
                                                                                  "Id", SessionStatus.CurrentSession.Id);

            db.AddToClientApplicationLog(clientApplicationLog);
            db.SaveChanges();

            return clientApplicationLog;
        }

        public void WriteClientClosedApplicationLog(ClientApplicationLog appLogOpened)
        {
            ClientApplicationLog clientApplicationLog = (from appLog in db.ClientApplicationLog
                                                         where appLog.Id == appLogOpened.Id
                                                         select appLog).FirstOrDefault();
            clientApplicationLog.EndDate = DateTime.Now;
            db.SaveChanges();
        }
    }
}
