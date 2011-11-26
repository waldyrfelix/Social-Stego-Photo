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
            Session["usuario_logado"] = FacebookHelper.ObterUsuarioFacebook(Request.Url);

            return redirecionaUsuario();
        }

        private ActionResult redirecionaUsuario()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
