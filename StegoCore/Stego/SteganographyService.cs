using StegoJpeg;
using System.Text;

namespace StegoCore.Stego
{
    public class SteganographyService : ISteganographyService
    {
        private readonly ICriptoService _criptoService;
        private readonly byte[] Key;
        private readonly byte[] IV;

        private StegoJpegFacade _stegoFacade;

        public SteganographyService(ICriptoService criptoService)
        {
            _criptoService = criptoService;
            _stegoFacade = new StegoJpegFacade();

            Key = Encoding.ASCII.GetBytes("APLICAÇÃO DE ESTEGANOGRAFIA EM I");
            IV = new byte[] { 245, 0, 23, 4, 53, 100, 255, 0, 14, 34, 55, 66, 1, 45, 1, 1 };
        }

        public void Embed(string path, string message)
        {
            byte[] cryptedMessage = _criptoService.Encrypt(message, Key, IV);
            _stegoFacade.EmbedData(path, cryptedMessage);
        }

        public string Extract(string path)
        {
            byte[] cryptedMessage = _stegoFacade.ExtractData(path);
            return _criptoService.Decrypt(cryptedMessage, Key, IV);
        }
    }
}
