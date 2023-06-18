using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API.Models.OrderAggregate
{
    public class Order : BaseEntity
    {
            public Order()
            {
            }

            public Order(IReadOnlyList<OrderItem> orderItems, AppUser user,Address shipToAddress, 
                DeliveryMethod deliveryMethod, decimal subtotal, string paymentIntentId)
            {
                User = user;
                BuyerEmail = user.Email;
                ShipToAddress = shipToAddress;
                DeliveryMethod = deliveryMethod;
                OrderItems = orderItems;
                Subtotal = subtotal;
                PaymentIntentId = paymentIntentId;
            }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public AppUser User { get; set; }
        
        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }
    }
}