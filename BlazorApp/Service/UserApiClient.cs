using System.Net.Http.Headers;
using BlazorApp.Data;

//写一个类型化的HttpClient，然后注册服务
namespace BlazorApp.Service
{
    public class UserApiClient
    {
        private readonly HttpClient _httpClient;
        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User[]?> GetForecastAsync()
        {

            var result = await _httpClient.GetFromJsonAsync<User[]>("/api/user/getall");

            return result;
        }
    }
}
