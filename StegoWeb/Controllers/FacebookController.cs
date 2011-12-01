using System.Web.Mvc;
using StegoCore;

namespace StegoWeb.Controllers
{
    public class FacebookController : Controller
    {
        private readonly IFacebookApiService _apiService;

        public FacebookController(IFacebookApiService apiService)
        {
            _apiService = apiService;
        }

        public ActionResult Login()
        {
            var facebookUrl = _apiService.Login();
            return Redirect(facebookUrl);
        }

        public ActionResult LoginRedirect()
        {
            var userData = _apiService.ObterUsuarioFacebook(Request.Url);
            Session["usuario_logado"] = userData;
            return redirecionaUsuario();
        }

        private ActionResult redirecionaUsuario()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
