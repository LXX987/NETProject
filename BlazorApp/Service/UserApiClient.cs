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
    }
}
