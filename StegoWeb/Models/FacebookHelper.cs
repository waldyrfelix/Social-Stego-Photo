using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Web;
using Facebook;

namespace StegoWeb.Models
{
    public class UsuarioFacebook
    {
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public long IdFacebook { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string UrlFacebook { get; set; }
        public string UrlImagemGrande { get; set; }
        public string UrlImagemPequena { get; set; }
    }

    public class FacebookHelper
    {
        public const string FacebookUrlReferrer = "FacebookUrlReferrer";

        private static Uri redirectUri;
        public static Uri MontarUrlDeRetorno()
        {
            if (redirectUri == null)
            {
                var url = HttpContext.Current.Request.Url;
                redirectUri = new Uri(url, "/Facebook/LoginRedirect");
            }

            return redirectUri;
        }


        protected static FacebookOAuthClient OAuthClient
        {
            get
            {
                return new FacebookOAuthClient(FacebookApplication.Current)
                {
                    RedirectUri = MontarUrlDeRetorno()
                };
            }
        }

        public static void Initialize()
        {
            FacebookApplication.SetApplication(new SocialStegoFacebookApp());
        }

        public static string Login()
        {
            var oauth = OAuthClient;
            var parametros = new Dictionary<string, object>
            {
                {"scope", "email,user_about_me,user_photos,publish_stream"},
            };

            return oauth.GetLoginUrl(parametros).ToString();
        }

        public static UsuarioFacebook ObterUsuarioFacebook(Uri url)
        {
            FacebookOAuthResult oauthResult;

            if (!FacebookOAuthResult.TryParse(url, out oauthResult))
            {
                throw new SocialStegoWebException("Url do facebook inválida");
            }

            if (!oauthResult.IsSuccess)
            {
                throw new SocialStegoWebException(oauthResult.ErrorReason + " - " + oauthResult.ErrorDescription);
            }

            dynamic tokenResult = OAuthClient.ExchangeCodeForAccessToken(oauthResult.Code);
            string accessToken = tokenResult.access_token;

            var fbClient = new FacebookClient(accessToken);

            dynamic me = fbClient.Get("me?fields=id,first_name,last_name,email,link");

            return new UsuarioFacebook
            {
                IdFacebook = Convert.ToInt64(me.id),
                AccessToken = accessToken,
                PrimeiroNome = me.first_name,
                UltimoNome = me.last_name,
                Email = me.email,
                UrlFacebook = me.link,
                UrlImagemGrande = String.Format("http://graph.facebook.com/{0}/picture?type=large", me.id),
                UrlImagemPequena = String.Format("http://graph.facebook.com/{0}/picture?type=small", me.id)
            };
        }

        public static void UploadPhoto(UsuarioFacebook usuarioFacebook, string path)
        {
            var mediaObject = new FacebookMediaObject
                                  {
                                      ContentType = "image/jpeg",
                                      FileName = Path.GetFileName(path)
                                  };

            byte[] bytes = File.ReadAllBytes(path);

            mediaObject.SetValue(bytes);

            var fb = new FacebookClient(usuarioFacebook.AccessToken);
            fb.PostCompleted += fb_PostCompleted;
            fb.PostAsync("/me/photos", new Dictionary<string, object> { { "source", mediaObject } });
        }

        private static void fb_PostCompleted(object sender, FacebookApiEventArgs e)
        {

        }
    }
}