
namespace Terra.API
{
    public interface IApi
    {
    }

    public class Api : IApi
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = Environment.GetEnvironmentVariable("ApiBaseUrl");

        public Api(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region API

        #endregion
    }
}
