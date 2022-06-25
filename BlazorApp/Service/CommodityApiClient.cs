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

namespace BlazorApp.Service
{
    public class CommodityApiClient
    {
        [Inject] public AuthenticationStateProvider AuthProvider { get; set; }
        private readonly HttpClient _httpClient;
        public CommodityApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Commodity[]?> GetAllCommodity()
        {

            var result = await _httpClient.GetFromJsonAsync<Commodity[]>("/api/temporaryOrder/getall");

            return result;
        }
        public async Task<OrderStr> Purchase(string token, string name, int num)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Commodity commodity = new Commodity();
            commodity.commodity_name = name;
            commodity.total_count = num;
            var result = await _httpClient.PostAsJsonAsync("/api/commodity/purchase", commodity);
            //Console.WriteLine(result.Content.ReadFromJsonAsync<Commodity>());// 好像是没用的
            var stringResponse = await result.Content.ReadAsStringAsync();// 靠这个
            Console.WriteLine(stringResponse);
            OrderStr order = await result.Content.ReadFromJsonAsync<OrderStr>();
            return order;
        }

        public async Task<commodityItem> OrderCommodity(int id)
        {
            TemporaryOrder order = new TemporaryOrder();
            order.commodity_id = id;
            var result = await _httpClient.PostAsJsonAsync("/api/temportaryOrder/findInformation", order);
            var stringResponse = await result.Content.ReadAsStringAsync();// 靠这个
            Console.WriteLine(stringResponse);
            commodityItem commodity = await result.Content.ReadFromJsonAsync<commodityItem>();
            Console.WriteLine("name:" + commodity.commodity_name);
            return commodity;
        }

        public async Task<commodityItem[]?> AllCommodity()
        {
            var result = await _httpClient.GetFromJsonAsync<commodityItem[]>("/api/commodity/getall");

            return result;
        }

        public async Task<int> addProduct(int price, int num,string name)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIyIiwiaWF0IjoiMTY1NjE1NTc5OSIsIm5iZiI6IjE2NTYxNTU3OTkiLCJleHAiOiIxNjU2MjQyMTk5IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9leHBpcmF0aW9uIjoiMjAyMi82LzI2IDE5OjE2OjM5IiwiaXNzIjoiand0SXNzdWVyIiwiYXVkIjoiand0QXVkaWVuY2UifQ.RJr3cZEcM-dZhcF12TNGcTp2ytj0oyAwVcFvofG8wD0");
            Commodity commodity = new Commodity();
            //commodityItem commodity = new commodityItem();
            commodity.commodity_id = 4;
            commodity.user_id = 2;
            string str = "矿泉水";
            commodity.commodity_name = name;
            commodity.item_price = 3;
            commodity.total_count = 500;
            commodity.start_time = new DateTime(2022, 6, 25, 11, 0, 0);
            commodity.end_time = new DateTime(2022, 6, 26, 11, 0, 0);
            var result = await _httpClient.PostAsJsonAsync("/api/commodity/postCommodity", commodity);
            var stringResponse = await result.Content.ReadAsStringAsync();// 靠这个
            Console.WriteLine(stringResponse);
            return 0;
        }
    }
}
