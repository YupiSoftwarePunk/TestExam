using Microsoft.EntityFrameworkCore;
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
                        p.Type,
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
