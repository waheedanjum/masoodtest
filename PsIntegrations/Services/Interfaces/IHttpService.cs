namespace PsIntegrations.Interfaces
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> PostAsync(string url, object requestBody);
    }
}
