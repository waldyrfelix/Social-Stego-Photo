namespace StegoCore
{
    public interface ISteganographyService
    {
        void Embed(string path, string message);
        string Extract(string path);
    }
}