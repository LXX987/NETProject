using BlazorApp.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorApp.Service
{
    public class UserApiService: IUserApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;
        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public UserApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<User>> GetHolidays(User holidaysRequest)
        {
            var result = new List<User>();

            var url = string.Format("api/user/getall");

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _clientFactory.CreateClient("UserApi");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();

                result = JsonSerializer.Deserialize<List<User>> (stringResponse,
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                result = Array.Empty<User>().ToList();
            }

            return result;
        }

        /*public async Task<UserType> Login(UserType user)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/user/login", user);
            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<UserType>(await response.Content.ReadAsStreamAsync());
            }
            else
            {
                throw new Exception("Gagal Tambah Data");
            }
        }*/
    }
}
