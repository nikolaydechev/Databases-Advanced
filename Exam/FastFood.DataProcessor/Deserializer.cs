namespace FastFood.DataProcessor
{
    using System;
    using FastFood.Data;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using FastFood.DataProcessor.Dto.Import;
    using FastFood.Models;
    using FastFood.Models.Enums;
    using Newtonsoft.Json;

    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            var deserializedEmployees = ImportJsons<EmployeeDto>(jsonString);

            var validEmployees = new List<Employee>();
            var validPositions = new List<Position>();

            var sb = new StringBuilder();

            foreach (var employeeDto in deserializedEmployees)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (!IsValid(employeeDto.Position))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var positionExists =
                validEmployees.Any(v => v.Position.Name == employeeDto.Position);

                Position position = null;
                if (!positionExists)
                {
                    position = new Position()
                    {
                        Name = employeeDto.Position
                    };
                }
                else
                {
                    position = validPositions.FirstOrDefault(p => p.Name == employeeDto.Position);
                }

                var employee = new Employee()
                {
                    Name = employeeDto.Name,
                    Age = employeeDto.Age,
                    Position = position
                };

                validEmployees.Add(employee);
                validPositions.Add(position);
                sb.AppendLine(string.Format(SuccessMessage, employeeDto.Name));
            }

            context.Employees.AddRange(validEmployees);

            context.Positions.AddRange(validPositions);

            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            var deserializedItems = ImportJsons<ItemDto>(jsonString);

            var validItems = new List<Item>();
            var validCategories = new List<Category>();

            var sb = new StringBuilder();

            foreach (var itemDto in deserializedItems)
            {
                if (itemDto.Price < 0.01m)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (!IsValid(itemDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var itemExists = validItems.Any(v => v.Name == itemDto.Name) || context.Items.Any(i => i.Name == itemDto.Name);

                if (itemExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var categoryExistsDb = context.Categories.Any(c => c.Name == itemDto.Category);

                var categoryExists = validCategories.Any(c => c.Name == itemDto.Category);

                Category category = null;
                if (!categoryExistsDb && !categoryExists)
                {
                    category = new Category()
                    {
                        Name = itemDto.Category
                    };

                    validCategories.Add(category);
                }

                if (categoryExists)
                {
                    category = validCategories.FirstOrDefault(c => c.Name == itemDto.Category);
                }
                else if (categoryExistsDb)
                {
                    category = context.Categories.FirstOrDefault(c => c.Name == itemDto.Category);
                }

                var item = new Item()
                {
                    Name = itemDto.Name,
                    Category = category,
                    Price = itemDto.Price
                };

                validItems.Add(item);
                sb.AppendLine(string.Format(SuccessMessage, itemDto.Name));
            }

            context.Items.AddRange(validItems);

            context.Categories.AddRange(validCategories);

            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {

            var serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));
            var deserializedOrders = (OrderDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();

            var validOrders = new List<Order>();
            var items = new List<Item>();

            foreach (var orderDto in deserializedOrders)
            {
                var employee = context.Employees.SingleOrDefault(e => e.Name == orderDto.Employee);
                if (employee == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (!IsValid(orderDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (!orderDto.Items.All(i => IsValid(i)))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (!orderDto.Items.All(i => context.Items.Any(oi => oi.Name == i.Name)))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var type = Enum.TryParse<OrderType>(orderDto.Type, out var orderType) ? orderType : OrderType.ForHere;

                var date = DateTime.ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                var orderItems = new List<OrderItem>();

                foreach (var i in orderDto.Items)
                {
                    orderItems.Add(new OrderItem()
                    {
                        Item = context.Items.Single(it => it.Name == i.Name),
                        Quantity = i.Quantity
                    });
                }

                var order = new Order()
                {
                    Customer = orderDto.Customer,
                    DateTime = date,
                    Type = type,
                    Employee = employee,
                    OrderItems = orderItems
                };
                
                validOrders.Add(order);

                sb.AppendLine($"Order for {orderDto.Customer} on {orderDto.DateTime} added");
            }

            context.Orders.AddRange(validOrders);

            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static T[] ImportJsons<T>(string jsonString)
        {
            T[] objects = JsonConvert.DeserializeObject<T[]>(jsonString);

            return objects;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return isValid;
        }
    }
}