using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StegoCore;
using StegoCore.Facebook;

namespace StegoWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISteganographyService _steganographyService;
        private readonly ISerializationService _serializationService;

        public HomeController(ISteganographyService steganographyService, ISerializationService serializationService)
        {
            _steganographyService = steganographyService;
            _serializationService = serializationService;
        }

        public FacebookUser FacebookUserLoggedUser
        {
            get
            {
                if (!IsAuthenticated)
                {
                    return null;
                }
                return Session["usuario_logado"] as FacebookUser;
            }
        }

        public bool IsAuthenticated
        {
            get { return Session["usuario_logado"] != null; }
        }


        public ActionResult Index()
        {
            string path = Server.MapPath("~/Arquivos");

            var files = Directory.GetFiles(path, "*_stego.jpg")
                .Select(f => "Arquivos/" + Path.GetFileName(f))
                .ToList();

            return View(files);
        }

        public ActionResult Upload()
        {
            if (!IsAuthenticated) return View("Authenticate");

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase photo)
        {
            if (!IsAuthenticated) return View("Authenticate");

            string path = makeFilePath();

            photo.SaveAs(path);

            string dataToBeEmbeded = serializeData();
            _steganographyService.Embed(path, dataToBeEmbeded);

            return RedirectToAction("UploadOk");
        }

        private string serializeData()
        {
            var facebookUser = FacebookUserLoggedUser;
            facebookUser.IP = Request.ServerVariables["REMOTE_ADDR"];
            facebookUser.UploadDate = DateTime.Now;

            return _serializationService.Serialize(facebookUser);
        }

        public ActionResult UploadOk()
        {
            if (!IsAuthenticated) return View("Authenticate");

            return View();
        }

        [ChildActionOnly]
        public ActionResult UsuarioLogado()
        {
            if (IsAuthenticated)
            {
                return PartialView("_UsuarioLogado");
            }
            return PartialView("_LoginUsuario");
        }

        private string makeFilePath()
        {
            string fileName = Guid.NewGuid().ToString("N");
            return Path.Combine(Server.MapPath("~/Arquivos"), fileName + ".jpg");
        }
    }
}