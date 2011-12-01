using System.Web.Mvc;
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
