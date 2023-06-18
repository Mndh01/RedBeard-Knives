using System.Collections.Generic;
using API.Models.OrderAggregate;

namespace API.DTOs.Admin
{
    public class OrderForAdminDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public string OrderDate { get; set; }
        public Models.OrderAggregate.Address ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}