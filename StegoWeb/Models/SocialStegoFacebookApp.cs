using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;

namespace StegoWeb.Models
{
    public class SocialStegoFacebookApp : IFacebookApplication
    {
        public string AppId
        {
            get { return "190026317751679"; }
        }

        public string AppSecret
        {
            get { return "e02ae2f4c23f534fdfe0cd720cc015a2"; }
        }

        public string CancelUrlPath
        {
            get { throw new NotImplementedException(); }
        }

        public string CanvasPage
        {
            get { throw new NotImplementedException(); }
        }

        public string CanvasUrl
        {
            get { throw new NotImplementedException(); }
        }

        public string SecureCanvasUrl
        {
            get { throw new NotImplementedException(); }
        }

        public string SiteUrl
        {
            get { throw new NotImplementedException(); }
        }

        public bool UseFacebookBeta
        {
            get { return false; }
        }
    }
}
