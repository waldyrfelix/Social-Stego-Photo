using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StegoJpeg;
using StegoWeb.Models;

namespace StegoWeb.Controllers
{
    public class HomeController : Controller
    {
        private StegoJpegFacade _stegoJpegFacade;

        public HomeController()
        {
            _stegoJpegFacade = new StegoJpegFacade();

        }

        public UsuarioFacebook UsuarioFacebook
        {
            get
            {
                if (Session["usuario_logado"] == null)
                {
                    return null;
                }
                return Session["usuario_logado"] as UsuarioFacebook;
            }
        }


        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }



        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase photo)
        {
            string fileName = Guid.NewGuid().ToString("N");
            string path = Path.Combine(Server.MapPath("~/Arquivos"), fileName + ".jpg");
            string pathStego = Path.Combine(Server.MapPath("~/Arquivos"), fileName + "_stego.jpg");
            photo.SaveAs(path);


            _stegoJpegFacade.EmbedData(path, "Teste de carga...");


            FacebookHelper.UploadPhoto(UsuarioFacebook, pathStego);

            return RedirectToAction("UploadOk");
        }


        public ActionResult UploadOk()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult UsuarioLogado()
        {
            if (Session["usuario_logado"] == null)
            {
                return PartialView("_LoginUsuario");
            }
            return PartialView("_UsuarioLogado");
        }
    }
}
