using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using StegoJpeg;
using StegoWeb.Models;

namespace StegoWeb.Controllers
{
    public class HomeController : Controller
    {
        private StegoJpegFacade _stegoJpegFacade;

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

        public HomeController()
        {
            _stegoJpegFacade = new StegoJpegFacade();

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
            _stegoJpegFacade.EmbedData(path, dataToBeEmbeded);

            return RedirectToAction("UploadOk");
        }

        private string serializeData()
        {
            var facebookUser = FacebookUserLoggedUser;
            facebookUser.IP = Request.ServerVariables["REMOTE_ADDR"];
            facebookUser.UploadDate = DateTime.Now;

            return JsonConvert.SerializeObject(facebookUser);
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
