using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using LanManager.BLL;

namespace LanManager.Web.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        public ActionResult LogOn()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(string userName, string password, bool? rememberMe, string returnUrl)
        {
            userName = userName.ToLower();

            if (!ValidateLogOn(userName, password))
            {
                return View();
            }

            FormsAuthentication.SetAuthCookie(userName, rememberMe ?? false);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        #region Validation Methods

        private bool ValidateLogOn(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "Você deve entrar com um usuário.");
            }
            if (String.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "Você deve entrar com a senha.");
            }
            if (!ValidateUser(userName, password))
            {
                ModelState.AddModelError("_FORM", "Nome de usuário ou senha inválidos.");
            }

            return ModelState.IsValid;
        }

        public bool ValidateUser(string userName, string password)
        {
            using (ClientManager context = new ClientManager())
            {
                DAL.Client client = context.SearchByUsername(userName, null);
                return client != null && client.Password == password;
            }
        }

        #endregion
    }
}
