using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Helpers;
using StegoWeb.Models;

namespace StegoWeb.Controllers
{
    public class FacebookController : Controller
    {
        public ActionResult Login()
        {
            var facebookUrl = FacebookHelper.Login();
            return Redirect(facebookUrl);
        }

        public ActionResult LoginRedirect()
        {
            var userData = FacebookHelper.ObterUsuarioFacebook(Request.Url);
            Session["usuario_logado"] = userData;
            return redirecionaUsuario();
        }

        private ActionResult redirecionaUsuario()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
