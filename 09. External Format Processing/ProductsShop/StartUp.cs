namespace ProductsShop
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
    using Microsoft.EntityFrameworkCore.Extensions.Internal;
    using Newtonsoft.Json;
    using ProductsShop.Data;
    using ProductsShop.Models;
    using Remotion.Linq.Parsing.Structure.IntermediateModel;

    public class StartUp
    {
        public static void Main()
        {
            // JSON
            //Console.WriteLine(ImportUsersFromJson());
            //Console.WriteLine(ImportCategoriesFromJson());
            //Console.WriteLine(ImportProductsFromJson());
            //SetCategoriesToProducts();
            //GetProductsInRangeToJson();
            //GetSuccessfullySoldProductsToJson();
            //GetCategoriesByProductsCountToJson();
            //GetUsersAndProductsToJson();

            // XML
            //Console.WriteLine(ImportUsersFromXml());
            //Console.WriteLine(ImportCategoriesFromXml());
            //Console.WriteLine(ImportProductsFromXml());
            //GetProductsInRangeToXml();
            //GetSoldProductsToXml();
            //GetCategoriesByProductsCountToXml();
            //GetUsersAndProductsToXml();
        }

        private static void GetUsersAndProductsToXml()
        {
            string path = "ExportedFiles/users-and-products.xml";

            using (var context = new ProductsShopContext())
            {
                var users = context.Users
                    .Where(u => u.SoldProducts.Count > 0)
                    .OrderByDescending(u => u.SoldProducts.Count)
                    .ThenBy(u => u.LastName)
                    .Select(u => new
                    {
                        FirstName = u.FirstName ?? "",
                        LastName = u.LastName ?? "",
                        u.Age,
                        u.SoldProducts
                    })
                    .ToArray();

                var xmlDoc = new XDocument(new XElement("users", new XAttribute("count", users.Length)));

                foreach (var u in users)
                {
                    string age = u.Age == null ? "" : u.Age.ToString();
                    var products = new List<XElement>();

                    foreach (var sp in u.SoldProducts)
                    {
                        products.Add(
                                new XElement("product", new XAttribute("name", sp.Name),
                                                        new XAttribute("price", sp.Price)));
                    }

                    xmlDoc.Root.Add(new XElement("user",
                        new XAttribute("first-name", u.FirstName),
                        new XAttribute("last-name", u.LastName),
                        new XAttribute("age", age),
                        new XElement("sold-products", 
                                    new XAttribute("count", u.SoldProducts.Count),
                                    products)));
                }

                using (var writer = new StreamWriter(path))
                {
                    xmlDoc.Save(writer);
                }
            }
        }

        private static void GetCategoriesByProductsCountToXml()
        {
            string path = "ExportedFiles/categories-by-products.xml";

            using (var context = new ProductsShopContext())
            {
                var categories = context.Categories
                    .OrderBy(c => c.CategoryProducts.Count)
                    .Select(c => new
                    {
                        c.Name,
                        NumberOfProducts = c.CategoryProducts.Count,
                        AvgPrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                        TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                    })
                    .ToArray();

                var xmlDoc = new XDocument(new XElement("categories"));

                foreach (var c in categories)
                {
                    xmlDoc.Root.Add(new XElement("category", new XAttribute("name", c.Name),
                                   new XElement("products-count", c.NumberOfProducts),
                                   new XElement("average-price", c.AvgPrice),
                                   new XElement("total-revenue", c.TotalRevenue)));
                }

                StreamWriter writer = new StreamWriter(path);
                using (writer)
                {
                    xmlDoc.Save(writer);
                }
            }
        }

        private static void GetSoldProductsToXml()
        {
            string path = "ExportedFiles/users-sold-products.xml";

            using (var context = new ProductsShopContext())
            {
                var users = context.Users
                    .Where(u => u.SoldProducts.Count > 0)
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Select(u => new
                    {
                        FirstName = u.FirstName ?? "",
                        u.LastName,
                        SoldProductName = u.SoldProducts.Select(s => s.Name),
                        SoldProductPrice = u.SoldProducts.Select(s => s.Price)
                    }).ToArray();

                var xmlDoc = new XDocument(new XElement("users"));

                foreach (var u in users)
                {
                    xmlDoc.Root.Add(new XElement("user",
                        new XAttribute("first-name", u.FirstName),
                        new XAttribute("last-name", u.LastName),
                            new XElement("sold-products",
                                 new XElement("products",
                                     new XElement("name", u.SoldProductName),
                                     new XElement("price", u.SoldProductPrice)))));
                }

                StreamWriter writer = new StreamWriter(path);
                using (writer)
                {
                    xmlDoc.Save(writer);
                }
            }
        }

        private static void GetProductsInRangeToXml()
        {
            string path = "ExportedFiles/products-in-range.xml";

            using (var context = new ProductsShopContext())
            {
                var products = context.Products
                    .Where(p => p.Price >= 1000 && p.Price <= 2000 && p.Buyer != null)
                    .OrderBy(p => p.Price)
                    .Select(p => new
                    {
                        p.Name,
                        p.Price,
                        Buyer = p.Buyer.FirstName == null
                                        ? p.Buyer.LastName
                                        : string.Concat(p.Buyer.FirstName, " ", p.Buyer.LastName)
                    })
                    .ToArray();

                var xmlDocument = new XDocument(new XElement("products"));

                foreach (var p in products)
                {
                    xmlDocument.Root?.Add(new XElement("product",
                        new XAttribute("name", p.Name),
                        new XAttribute("price", p.Price),
                        new XAttribute("buyer", p.Buyer)));
                }

                StreamWriter writer = new StreamWriter(path);
                using (writer)
                {
                    xmlDocument.Save(writer);
                }

                //File.WriteAllText(path, xmlDocument.ToString());
            }
        }

        private static string ImportCategoriesFromXml()
        {
            string path = "Files/categories.xml";
            string xmlString = File.ReadAllText(path);

            var xmlDoc = XDocument.Parse(xmlString);
            var elements = xmlDoc.Root.Elements();

            var categories = new List<Category>();

            foreach (var e in elements)
            {
                string name = e.Element("name")?.Value;

                var category = new Category()
                { Name = name };

                categories.Add(category);
            }

            using (var context = new ProductsShopContext())
            {
                context.Categories.AddRange(categories);

                context.SaveChanges();
            }

            return $"{categories.Count} categories were imported from: {path}";
        }

        private static string ImportProductsFromXml()
        {
            string path = "Files/products.xml";
            string xmlString = File.ReadAllText(path);

            var xmlDoc = XDocument.Parse(xmlString);
            var elements = xmlDoc.Root.Elements();

            var categoryProducts = new List<CategoryProduct>();
            Random random = new Random();

            using (var context = new ProductsShopContext())
            {
                var usersIds = context.Users.Select(u => u.Id).ToArray();
                var categoryIds = context.Categories.Select(c => c.Id).ToArray();

                foreach (var e in elements)
                {
                    string name = e.Element("name")?.Value;
                    decimal price = decimal.Parse(e.Element("price").Value);

                    int buyerIndex = random.Next(0, usersIds.Length);
                    int? buyerId = usersIds[buyerIndex];
                    int sellerIndex = random.Next(0, usersIds.Length);
                    int sellerId = usersIds[sellerIndex];

                    while (buyerId == sellerId)
                    {
                        sellerIndex = random.Next(0, usersIds.Length);
                        sellerId = usersIds[sellerIndex];
                    }
                    if (buyerId - sellerId < 4 && buyerId - sellerId > 0)
                    {
                        buyerId = null;
                    }

                    int categoryIndex = random.Next(0, categoryIds.Length);
                    int categoryId = categoryIds[categoryIndex];

                    var product = new Product()
                    {
                        BuyerId = buyerId,
                        Name = name,
                        Price = price,
                        SellerId = sellerId
                    };

                    var categoryProduct = new CategoryProduct()
                    {
                        Product = product,
                        CategoryId = categoryId
                    };

                    categoryProducts.Add(categoryProduct);
                }

                context.CategoryProduct.AddRange(categoryProducts);

                context.SaveChanges();
            }

            return $"{categoryProducts.Count} products were imported from: {path}";
        }

        private static string ImportUsersFromXml()
        {
            string path = "Files/users.xml";

            string xmlString = File.ReadAllText(path);

            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();

            var users = new List<User>();

            foreach (var e in elements)
            {
                string firstName = e.Attribute("firstName")?.Value;
                string lastName = e.Attribute("lastName")?.Value;

                int? age = e.Attribute("age")?.Value != null
                    ? int.Parse(e.Attribute("age")?.Value)
                    : default(int?);

                var user = new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age
                };

                users.Add(user);
            }

            using (var context = new ProductsShopContext())
            {
                context.Users.AddRange(users);

                context.SaveChanges();
            }

            return $"{users.Count} users were imported from file: {path}";
        }

        private static void GetUsersAndProductsToJson()
        {
            string path = "ExportedFiles/users-and-products.json";

            using (var context = new ProductsShopContext())
            {
                var users = context.Users
                    .Where(u => u.SoldProducts.Count > 0)
                    .OrderByDescending(u => u.SoldProducts.Count)
                    .ThenBy(u => u.LastName)
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        u.Age,
                        SoldProducts = u.SoldProducts.Select(s => new
                        {
                            s.Name,
                            s.Price
                        })
                    })
                    .ToArray();

                var usersToSerialize = new
                {
                    UsersCount = users.Length,
                    Users = users
                };

                ExportJsons(path, usersToSerialize);
            }
        }

        private static void GetCategoriesByProductsCountToJson()
        {
            string path = "ExportedFiles/categories-by-products.json";

            using (var context = new ProductsShopContext())
            {
                var categories = context.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new
                    {
                        c.Name,
                        ProductsCount = c.CategoryProducts.Count,
                        AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                        TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                    })
                    .ToArray();

                ExportJsons(path, categories);
            }
        }

        private static void GetSuccessfullySoldProductsToJson()
        {
            string path = "ExportedFiles/users-sold-products.json";

            using (var context = new ProductsShopContext())
            {
                var users = context.Users
                    .Where(u => u.SoldProducts.Count > 0)
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        SoldProducts = u.SoldProducts.Where(s => s.Buyer != null).Select(s => new
                        {
                            s.Name,
                            s.Price,
                            s.Buyer.FirstName,
                            s.Buyer.LastName
                        }).ToArray()
                    })
                    .ToArray();

                ExportJsons(path, users);
            }
        }

        private static void GetProductsInRangeToJson()
        {
            string path = @"ExportedFiles/products-in-range.json";

            using (var context = new ProductsShopContext())
            {
                var products = context.Products
                    .Where(p => p.Price >= 500 && p.Price <= 1000)
                    .OrderBy(p => p.Price)
                    .Select(p => new
                    {
                        p.Name,
                        p.Price,
                        SellerFullname = p.Seller.FirstName + " " + p.Seller.LastName
                    })
                    .ToArray();

                ExportJsons(path, products);
            }
        }

        private static void SetCategoriesToProducts()
        {
            using (var context = new ProductsShopContext())
            {
                var productIds = context.Products.Select(p => p.Id).ToArray();
                var categoryIds = context.Categories.Select(c => c.Id).ToArray();

                Random random = new Random();

                var categoryProducts = new List<CategoryProduct>();

                foreach (var productId in productIds)
                {
                    int categoryIndex = random.Next(0, categoryIds.Length);
                    int randomCategoryId = categoryIds[categoryIndex];

                    while (categoryProducts.Any(cp => cp.CategoryId == randomCategoryId && cp.ProductId == productId))
                    {
                        categoryIndex = random.Next(0, categoryIds.Length);
                        randomCategoryId = categoryIds[categoryIndex];
                    }

                    CategoryProduct categoryProduct = new CategoryProduct
                    {
                        CategoryId = randomCategoryId,
                        ProductId = productId
                    };

                    categoryProducts.Add(categoryProduct);
                }

                context.CategoryProduct.AddRange(categoryProducts);

                context.SaveChanges();
            }
        }

        private static string ImportProductsFromJson()
        {
            string path = "Files/products.json";

            var products = ImportJsons<Product>(path);

            Random random = new Random();

            using (var context = new ProductsShopContext())
            {
                int[] userIds = context.Users.Select(u => u.Id).ToArray();

                foreach (var product in products)
                {
                    int index = random.Next(0, userIds.Length);
                    int sellerId = userIds[index];

                    int? buyerId = sellerId;
                    while (buyerId == sellerId)
                    {
                        int buyerIndex = random.Next(0, userIds.Length);
                        buyerId = userIds[buyerIndex];
                    }

                    if (buyerId - sellerId < 4 && buyerId - sellerId > 0)
                    {
                        buyerId = null;
                    }

                    product.SellerId = sellerId;
                    product.BuyerId = buyerId;
                }

                context.Products.AddRange(products);

                context.SaveChanges();
            }

            return $"{products.Length} products were imported from: {path}";
        }

        private static string ImportCategoriesFromJson()
        {
            string path = "Files/categories.json";
            var categories = ImportJsons<Category>(path);

            using (var context = new ProductsShopContext())
            {
                context.Categories.AddRange(categories);

                context.SaveChanges();
            }

            return $"{categories.Length} categories were imported from: {path}";
        }

        private static string ImportUsersFromJson()
        {
            string path = "Files/users.json";
            var users = ImportJsons<User>(path);

            using (var context = new ProductsShopContext())
            {
                context.Users.AddRange(users);

                context.SaveChanges();
            }

            return $"{users.Length} users were imported from: {path}";
        }

        private static T[] ImportJsons<T>(string path)
        {
            string jsonString = File.ReadAllText(path);

            T[] objects = JsonConvert.DeserializeObject<T[]>(jsonString);

            return objects;
        }

        private static void ExportJsons<T>(string path, params T[] data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings()
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            });

            System.IO.File.WriteAllText(path, json);
        }
    }
}
