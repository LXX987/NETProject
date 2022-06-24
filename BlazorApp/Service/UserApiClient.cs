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

        /*public async ValueTask<UserType> Login(string email, string psw, string type)
        {
            UserType userType=new UserType { user_email=email, user_pwd=psw,userType=type};
            Console.WriteLine(userType.user_email);
            Console.WriteLine(userType.user_pwd);
            Console.WriteLine(userType.userType);
            var response = await _httpClient.PostAsJsonAsync<UserType>("/api/user/login", userType);
            var result= response.Content.ReadFromJsonAsync<UserType>();
            Console.WriteLine(result);
            return result;
        }*/
    }
}
