using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Web;
using Facebook;

namespace StegoWeb.Models
{
    public class FacebookUser
    {
        public string Picture { get; set; }
        public long IdFacebook { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Hometown { get; set; }
        public string Location { get; set; }
        public string Gender { get; set; }
        public string IP { get; set; }
        public DateTime UploadDate { get; set; }
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
                {"scope", "email,user_about_me"},
            };

            return oauth.GetLoginUrl(parametros).ToString();
        }

        public static FacebookUser ObterUsuarioFacebook(Uri url)
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

            dynamic me = fbClient.Get("me?fields=?fields=id,name,email,link,username,hometown,location,gender");

            return new FacebookUser
            {
                IdFacebook = Convert.ToInt64(me.id),
                AccessToken = accessToken,
                Email = me.email,
                Name = me.name,
                Picture = String.Format("http://graph.facebook.com/{0}/picture?type=small", me.id),
                Link = me.link,
                Username = me.username,
                Hometown = me.hometown.name,
                Location = me.location.name,
                Gender = me.gender,
            };
        }
    }
}