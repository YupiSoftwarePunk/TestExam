using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class HttpCLientService
    {
        private readonly HttpClient httpCLient;
        private readonly string url;
        public HttpCLientService(string baseUrl)
        {
            url = baseUrl.TrimEnd('/');
            httpCLient = new HttpClient();
        }

        public async Task<List<Parthners>?> getParthners()
        {
            var response = await httpCLient.GetAsync($"{url}/parthners");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Parthners>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }


        //public async Task<List<ParthnersProductsResponse>?> getParthners()
        //{
        //    var response = await httpCLient.GetAsync($"{url}/parthners");
        //    response.EnsureSuccessStatusCode();

        //    var content = await response.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<List<ParthnersProductsResponse>>(content, new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    });
        //}


        public async Task<List<Products>?> GetTotalProducts()
        {
            var response = await httpCLient.GetAsync($"{url}/products");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Products>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
