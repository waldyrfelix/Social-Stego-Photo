using System;

namespace StegoCore.Facebook
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
}