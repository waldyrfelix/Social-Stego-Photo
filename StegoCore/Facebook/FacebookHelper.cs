using Facebook;

namespace StegoCore.Facebook
{
    public class FacebookHelper
    {
        public static void Initialize()
        {
            FacebookApplication.SetApplication(new SocialStegoFacebookApp());
        }
    }
}