using System;
using StegoCore.Facebook;

namespace StegoCore
{
    public interface IFacebookApiService
    {
        string Login();
        FacebookUser ObterUsuarioFacebook(Uri url);
    }
}