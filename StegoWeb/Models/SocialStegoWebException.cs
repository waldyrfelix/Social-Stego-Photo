using System;

namespace StegoWeb.Models
{
    public class SocialStegoWebException : Exception
    {
        public SocialStegoWebException(string mensagem)
            : base(mensagem)
        {

        }
    }
}