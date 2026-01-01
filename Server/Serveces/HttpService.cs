using Microsoft.EntityFrameworkCore;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server.Serveces
{
    public class HttpService
    {
        public static HttpListener listener = new HttpListener();

        public static void Start()
        {
            listener.Prefixes.Add("http://localhost:5070/api/");
            listener.Start();
            Console.WriteLine("Сервер запущен и ожидает подключения на порту 5070");

            Task.Run(async () =>
            {
                while (true)
                {
                    var context = listener.GetContext();
                    await HandleRequest(context);
                }
            });
        }

        public static async Task HandleRequest(HttpListenerContext context)
        {
            string path = context.Request.Url.AbsolutePath;
            string method = context.Request.HttpMethod;

            Console.WriteLine($"путь {path}, метод {method}");

            using var db = new DbAppContext();

            if (path == "/api/parthners" && method == "GET")
            {
                var parthners = db.Parthners
                    .Include(p => p.ProductEntities)
                    .Select(p => new
                    {
                        p.Id,
                        p.PhoneNumber,
                        p.PartnerTypeId,
                        p.DirectorFullName,
                        p.Rating,
                        p.ComapnyName
                    }).ToList();

                //var products = db.Products
                //    .Select(p => new
                //    {
                //        p.Id,
                //        p.TotalProducts,
                //        p.ParthnerId
                //    })
                //    .ToList();

                //var response = new
                //{
                //    Parthners = parthners,
                //    Products = products
                //};

                string responseBody = System.Text.Json.JsonSerializer.Serialize(parthners);
                await WriteResponse(context, responseBody);
            }
            else if (path == "/api/parthners" && method == "POST")
            {
                using var reader = new StreamReader(context.Request.InputStream);
                string body = await reader.ReadToEndAsync();

                var newPartner = System.Text.Json.JsonSerializer.Deserialize<Parthners>(body);

                if (newPartner == null)
                {
                    await WriteResponse(context, "Invalid JSON", 400);
                    return;
                }

                db.Parthners.Add(newPartner);
                db.SaveChanges();

                await WriteResponse(context, "{\"status\":\"created\"}", 201);
                return;
            }
            else if (path.StartsWith("/api/parthners/") && method == "PUT")
            {
                int id = int.Parse(path.Split('/').Last());

                var partner = db.Parthners.FirstOrDefault(p => p.Id == id);
                if (partner == null)
                {
                    await WriteResponse(context, "Not Found", 404);
                    return;
                }

                using var reader = new StreamReader(context.Request.InputStream);
                string body = await reader.ReadToEndAsync();

                var updated = System.Text.Json.JsonSerializer.Deserialize<Parthners>(body);

                partner.ComapnyName = updated.ComapnyName;
                partner.PhoneNumber = updated.PhoneNumber;
                partner.Address = updated.Address;
                partner.DirectorFullName = updated.DirectorFullName;
                partner.Email = updated.Email;
                partner.Rating = updated.Rating;
                partner.PartnerTypeId = updated.PartnerTypeId;

                db.SaveChanges();

                await WriteResponse(context, "{\"status\":\"updated\"}", 200);
                return;
            }
            else if (path.StartsWith("/api/parthners/") && method == "DELETE")
            {
                int id = int.Parse(path.Split('/').Last());

                var partner = db.Parthners.FirstOrDefault(p => p.Id == id);
                if (partner == null)
                {
                    await WriteResponse(context, "Not Found", 404);
                    return;
                }

                db.Parthners.Remove(partner);
                db.SaveChanges();

                await WriteResponse(context, "{\"status\":\"deleted\"}", 200);
                return;
            }

            else if (path == "/api/products" && method == "GET")
            {
                var products = db.Products
                    .Include(p => p.Parthner)
                    .Select(p => new
                    {
                        p.Id,
                        p.TotalProducts,
                        p.ParthnerId
                    });
                string responseBody = System.Text.Json.JsonSerializer.Serialize(products);
                await WriteResponse(context, responseBody);
            }
            if (path == "/api/products" && method == "POST")
            {
                using var reader = new StreamReader(context.Request.InputStream);
                string body = await reader.ReadToEndAsync();

                var newProduct = System.Text.Json.JsonSerializer.Deserialize<Products>(body);

                if (newProduct == null)
                {
                    await WriteResponse(context, "Invalid JSON", 400);
                    return;
                }

                db.Products.Add(newProduct);
                db.SaveChanges();

                await WriteResponse(context, "{\"status\":\"created\"}", 201);
                return;
            }
            if (path.StartsWith("/api/products/") && method == "PUT")
            {
                int id = int.Parse(path.Split('/').Last());

                var product = db.Products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                {
                    await WriteResponse(context, "Not Found", 404);
                    return;
                }

                using var reader = new StreamReader(context.Request.InputStream);
                string body = await reader.ReadToEndAsync();

                var updated = System.Text.Json.JsonSerializer.Deserialize<Products>(body);

                product.Name = updated.Name;
                product.TypeId = updated.TypeId;
                product.MaterialId = updated.MaterialId;
                product.TotalProducts = updated.TotalProducts;
                product.Height = updated.Height;
                product.Width = updated.Width;
                product.ParthnerId = updated.ParthnerId;
                product.SaleDate = updated.SaleDate;

                db.SaveChanges();

                await WriteResponse(context, "{\"status\":\"updated\"}", 200);
                return;
            }
            if (path.StartsWith("/api/products/") && method == "DELETE")
            {
                int id = int.Parse(path.Split('/').Last());

                var product = db.Products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                {
                    await WriteResponse(context, "Not Found", 404);
                    return;
                }

                db.Products.Remove(product);
                db.SaveChanges();

                await WriteResponse(context, "{\"status\":\"deleted\"}", 200);
                return;
            }

            else
            {
                await WriteResponse(context, "Not Found", 404);
            }
        }


        public static async Task WriteResponse(HttpListenerContext context, string body, int status = 200)
        {
            context.Response.StatusCode = status;
            byte[] buffer = Encoding.UTF8.GetBytes(body);
            context.Response.ContentType = "application/json";
            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            context.Response.Close();
        }
    }
}
