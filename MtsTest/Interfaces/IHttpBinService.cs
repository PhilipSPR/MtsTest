namespace MtsTest.Interfaces
{
    public interface IHttpBinService
    {
        Task<HttpResponseMessage> SendImageAsync(string imageBase64);
    }
}
