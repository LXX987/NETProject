using System.Net.Http.Headers;
using BlazorApp.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

//写一个类型化的HttpClient，然后注册服务
namespace BlazorApp.Service
{
    public class UserApiClient
    {
        [Inject] public AuthenticationStateProvider AuthProvider { get; set; }
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

        public async Task<int> searchCommodityt(string commodity_name)
        {
            Console.WriteLine(commodity_name);
            Commodity commodity = new Commodity();
            commodity.commodity_name = commodity_name;
            var result = await _httpClient.PostAsJsonAsync("/api/commodity/searchCommodity", commodity);
            Console.WriteLine(result);
            Console.WriteLine(result.Content.ReadFromJsonAsync<Commodity>());// 好像是没用的
            var stringResponse = await result.Content.ReadAsStringAsync();// 靠这个
            Console.WriteLine(stringResponse);
            /*Console.WriteLine(JsonSerializer.Deserialize<Commodity>(stringResponse));*/
            return 0;
        }

        public async Task<int> login(string email, string psw, string type)
        {
            Console.WriteLine(email);
            Console.WriteLine(psw);
            Console.WriteLine(type);
            User user=new User();
            user.user_email = email;
            user.user_pwd = psw;
            user.userType = type;
            user.userName = "";
            var httpResponse = await _httpClient.PostAsJsonAsync("/api/user/login", user);
            Console.WriteLine(httpResponse);
            //Console.WriteLine(httpResponse.Content.ReadFromJsonAsync<User>());// 好像是没用的
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();// 靠这个
            UserDto result = await httpResponse.Content.ReadFromJsonAsync<UserDto>();

            if (string.IsNullOrWhiteSpace(result?.Token) == false)
            {
                Console.WriteLine("登录成功");
                ((AuthProvider)AuthProvider).MarkUserAsAuthenticated(result);
                //AuthProvider authProvider = new AuthProvider(_httpClient);
                //authProvider.MarkUserAsAuthenticated(result);
                //(AuthProvider).MarkUserAsAuthenticated(result);
            }
            else
            {
                Console.WriteLine("用户名或密码错误");
            }
            Console.WriteLine(stringResponse);
            return 0;
        }

        public async Task<User> SearchUser(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpResponse = await _httpClient.GetFromJsonAsync<User>("/api/user/searchUser");
            return httpResponse;
        }

        public async Task<TemporaryOrder[]?> GetOrderList(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpResponse = await _httpClient.GetFromJsonAsync<TemporaryOrder[]>("/api/user/searchUser");
            return httpResponse;
        }
    }
}
