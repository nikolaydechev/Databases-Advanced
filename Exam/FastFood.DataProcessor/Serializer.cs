namespace FastFood.DataProcessor
{
    using System;
    using System.IO;
    using FastFood.Data;
    using System.Linq;
    using System.Xml.Linq;
    using FastFood.DataProcessor.Dto.Export;
    using FastFood.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {

            var type = Enum.Parse<OrderType>(orderType);

            var orders = context.Orders
                .Where(o => o.Employee.Name == employeeName && o.Type == type)
                .Select(o => new
                {
                    Customer = o.Customer,
                    Items = o.OrderItems.Select(x => new
                    {
                        Name = x.Item.Name,
                        Price = x.Item.Price,
                        Quantity = x.Quantity
                    })
                    .ToArray(),
                    TotalPrice = o.TotalPrice
                })
                .OrderByDescending(o => o.TotalPrice)
                .ThenByDescending(o => o.Items.Length)
                .ToArray();

            var totalMoneyMade = orders.Sum(o => o.TotalPrice);

            var employeeOrders = new
            {
                Name = employeeName,
                Orders = orders,
                TotalMade = totalMoneyMade
            };

            var json = JsonConvert.SerializeObject(employeeOrders, Formatting.Indented
            );

            return json;

            //return ExportJsons(employeeOrders);
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            //var categoryNames = categoriesString.Split(',');

            //foreach (var categoryName in categoryNames)
            //{
            //    var category = context.Categories.Include(c=>c.Items).ThenInclude(i=>i.OrderItems).FirstOrDefault(c => c.Name == categoryName);

            //    var mostPopularItem = category.Items
            //        .Select(i => i.OrderItems.Max(oi => oi.Item.Price * oi.Quantity))
            //        .Max();

            //    Console.WriteLine();
            //}
            return "";
        }

        private static string ExportJsons<T>(params T[] data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented
            );

            return json;
        }
    }
}