using System;
using System.Collections.Generic;
using System.Web;
using Facebook;

namespace StegoCore.Facebook
{
    public class FacebookApiService : IFacebookApiService
    {
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

        protected FacebookOAuthClient OAuthClient
        {
            get
            {
                return new FacebookOAuthClient(FacebookApplication.Current)
                {
                    RedirectUri = MontarUrlDeRetorno()
                };
            }
        }

        public string Login()
        {
            var oauth = OAuthClient;
            var parametros = new Dictionary<string, object>
            {
                {"scope", "email,user_about_me"},
            };

            return oauth.GetLoginUrl(parametros).ToString();
        }

        public FacebookUser ObterUsuarioFacebook(Uri url)
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