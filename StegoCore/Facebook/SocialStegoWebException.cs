using System;

namespace StegoCore.Facebook
{
    public class SocialStegoWebException : Exception
    {
        public SocialStegoWebException(string mensagem) : base(mensagem) { }
    }
}