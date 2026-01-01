using Client.Models;
using System;
using System.Collections;
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

        public async Task<List<Parthners>> GetParthners()
        {
            var response = await httpCLient.GetAsync($"{url}/parthners");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Parthners>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }


        //public async Task<List<ParthnersProductsResponse>?> GetParthners()
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


        public async Task<bool> AddPartner(Parthners partner)
        {
            var json = JsonSerializer.Serialize(partner);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpCLient.PostAsync($"{url}/parthners", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdatePartner(int id, Parthners partner)
        {
            var json = JsonSerializer.Serialize(partner);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpCLient.PutAsync($"{url}/parthners/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePartner(int id)
        {
            var response = await httpCLient.DeleteAsync($"{url}/parthners/{id}");
            return response.IsSuccessStatusCode;
        }


        public async Task<bool> AddProduct(Products product)
        {
            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpCLient.PostAsync($"{url}/products", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProduct(int id, Products product)
        {
            var json = JsonSerializer.Serialize(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpCLient.PutAsync($"{url}/products/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var response = await httpCLient.DeleteAsync($"{url}/products/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<PartnerType>> GetPartnerTypes()
        {
            var response = await httpCLient.GetAsync($"{url}/partnertypes");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<PartnerType>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

    }
}
