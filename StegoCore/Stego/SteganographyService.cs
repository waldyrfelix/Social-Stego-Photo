using StegoJpeg;

namespace StegoCore.Stego
{
    public class SteganographyService : ISteganographyService
    {
        private StegoJpegFacade _stegoFacade;

        public SteganographyService()
        {
            _stegoFacade = new StegoJpegFacade();
        }

        public void Embed(string path, string message)
        {
            _stegoFacade.EmbedData(path, message);
        }

        public string Extract(string path)
        {
            return _stegoFacade.ExtractData(path);
        }
    }
}
