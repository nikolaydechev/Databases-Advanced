namespace FastFood.Models
{
    using System;
    using System.Collections.Generic;
    using FastFood.Models.Enums;

    public class Order
    {
        public int Id { get; set; }

        public string Customer { get; set; }

        public DateTime DateTime { get; set; }

        public OrderType Type { get; set; } = OrderType.ForHere;

        public decimal TotalPrice { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
