using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LanManager.BLL;

namespace LanManager.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            Client client;
            using (ClientManager context = new ClientManager())
            {
                client = context.SearchByUsername(User.Identity.Name, new[] {"Dependents"});
            }

            return View(client.Dependents.Where(x=>x.BirthDate > DateTime.Now.AddYears(-18)).ToList());
        }

        public ActionResult EditarApps(string id)
        {
            Client client;
            List<Application> allApps;
            List<ApplicationGroup> allGorups;
            using (ClientManager context = new ClientManager())
            {
                client = context.GetClientsById(new Guid(id), new[] {"ApplicationsAllowed", "ApplicationGroupsAllowed", "Parent"});
            }

            if (string.Compare(client.Parent.UserName, User.Identity.Name, StringComparison.CurrentCultureIgnoreCase) != 0)
            {
                return RedirectToAction("Index");
            }

            using (ApplicationManager context = new ApplicationManager())
            {
                allApps = context.SearchAllApplications(null).Where(x=>x.Active).ToList();
                allGorups = context.SearchAllGroups(null);
            }
            ViewData["AllApps"] = allApps;
            ViewData["AllGroups"] = allGorups;

            return View(client);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditarApps(string id, bool canAccessAllApps)
        {
            Client client;
            using (ClientManager context = new ClientManager())
            {
                client = context.GetClientsById(new Guid(id), null);

                client.CanAccessAnyApplication = canAccessAllApps;

                context.SaveChanges();
            }

            return RedirectToAction("EditarApps", new {id = client.Id.ToString()});
        }

        public ActionResult AdicionarApp(string id, string app)
        {
            Client client;
            using (ClientManager context = new ClientManager())
            {
                client = context.GetClientsById(new Guid(id), null);

                if (!string.IsNullOrEmpty(app))
                {
                        Application addedApp;
                        using (ApplicationManager contextApp = new ApplicationManager(context.db))
                        {
                            addedApp = contextApp.GetApplicationById(new Guid(app), null);
                        }

                        client.ApplicationsAllowed.Add(addedApp);
                }

                context.SaveChanges();
            }

            return RedirectToAction("EditarApps", new { id = client.Id.ToString() });
        }

        public ActionResult RemoverApp(string id, string app)
        {
            Client client;
            using (ClientManager context = new ClientManager())
            {
                client = context.GetClientsById(new Guid(id), new[] { "ApplicationsAllowed" });

                if (!string.IsNullOrEmpty(app))
                {
                    client.ApplicationsAllowed.Remove(client.ApplicationsAllowed.First(x => x.Id == new Guid(app)));
                }

                context.SaveChanges();
            }

            return RedirectToAction("EditarApps", new { id = client.Id.ToString() });
        }

        public ActionResult AdicionarGroup(string id, string app)
        {
            Client client;
            using (ClientManager context = new ClientManager())
            {
                client = context.GetClientsById(new Guid(id), new[] { "ApplicationGroupsAllowed" });

                if (!string.IsNullOrEmpty(app))
                {
                    ApplicationGroup addedGroup;
                    using (ApplicationManager contextApp = new ApplicationManager(context.db))
                    {
                        addedGroup = contextApp.SearchGroupById(new Guid(app), null);
                    }

                    client.ApplicationGroupsAllowed.Add(addedGroup);
                }

                context.SaveChanges();
            }

            return RedirectToAction("EditarApps", new { id = client.Id.ToString() });
        }

        public ActionResult RemoverGroup(string id, string app)
        {
            Client client;
            using (ClientManager context = new ClientManager())
            {
                client = context.GetClientsById(new Guid(id), new[] { "ApplicationGroupsAllowed" });

                if (!string.IsNullOrEmpty(app))
                {
                    client.ApplicationGroupsAllowed.Remove(client.ApplicationGroupsAllowed.First(x => x.Id == new Guid(app)));
                }

                context.SaveChanges();
            }

            return RedirectToAction("EditarApps", new { id = client.Id.ToString() });
        }

        public ActionResult EditarHorarios(string id)
        {
            Client client;

            using (ClientManager context = new ClientManager())
            {
                client = context.GetClientsById(new Guid(id), new[] { "PeriodAllowed", "Parent" });
            }

            if (string.Compare(client.Parent.UserName, User.Identity.Name, StringComparison.CurrentCultureIgnoreCase) != 0)
            {
                return RedirectToAction("Index");
            }

            return View(client);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditarHorarios(string id, bool canAccessAnyTime)
        {
            Client client;

            using (ClientManager context = new ClientManager())
            {
                client = context.GetClientsById(new Guid(id), null);
                client.CanAccessAnyTime = canAccessAnyTime;

                context.SaveChanges();
            }

            return RedirectToAction("EditarHorarios", new {id = id});
        }

        public ActionResult RemoverHorario(string id, string periodAllowedId)
        {
            using (PermissionManager context = new PermissionManager())
            {
                context.RemovePeriodAllowed(new Guid(periodAllowedId));
            }

            return RedirectToAction("EditarHorarios", new {id});
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AdicionarHorario(string id, string dayOfWeek, string startHour, string endHour)
        {
            Client client;

            int? diaDasemana = null;
            int aux;
            if (int.TryParse(dayOfWeek, out aux))
                diaDasemana = aux;

            using (ClientManager context = new ClientManager())
            {
                client = context.GetClientsById(new Guid(id), new[] {"PeriodAllowed"});

                int horaInicio;
                int horaFim;
                if (!int.TryParse(startHour, out horaInicio) || !int.TryParse(endHour, out horaFim) || horaInicio < 0 || horaFim > 24)
                {
                    ModelState.AddModelError("horarios", "Horário inválido");
                    return View("EditarHorarios", client);
                }

                if (horaInicio >= horaFim || horaFim < horaInicio)
                {
                    ModelState.AddModelError("horarios", "Horário de inicío deve ser menor que o de fim");
                    return View("EditarHorarios", client);
                }

                client.PeriodAllowed.Add(new PeriodAllowed
                                             {
                                                 Id = Guid.NewGuid(),
                                                 DayOfWeek = diaDasemana,
                                                 StartHour = horaInicio,
                                                 EndHour = horaFim
                                             });

                context.SaveChanges();
            }

            return RedirectToAction("EditarHorarios", new {id = client.Id});
        }

        public ActionResult Relatorio(string id)
        {
            Client client;
            using (ClientManager context = new ClientManager())
            {
                client = context.GetClientsById(new Guid(id), new[] {"ClientSession"});
            }

            return View(client);
        }

        public ActionResult VerSessao(string id, string sessao)
        {
            Client client;
            using (ClientManager context = new ClientManager())
            {
                client = context.GetClientsById(new Guid(id), new[] { "ClientSession", "ClientSession.ClientApplicationLog", "ClientSession.ClientApplicationLog.Application" });
            }

            return View(client.ClientSession.First(x=>x.Id == new Guid(sessao)));
        }
    }
}
