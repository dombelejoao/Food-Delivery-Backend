using System;

namespace FoodDelivery.DataAccess.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid MenuItemId { get; set; }
        public int Quantity { get; set; }

        // ✅ Add this property if it doesn’t exist yet
        public decimal Price { get; set; }

        // Relationships
        public Order? Order { get; set; }
        public MenuItem? MenuItem { get; set; }
    }
}
