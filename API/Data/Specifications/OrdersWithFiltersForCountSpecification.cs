using System;
using API.Models.OrderAggregate;

namespace API.Data.Specifications
{
    public class OrdersWithFiltersForCountSpecification : BaseSpecification<Order>
    {
        public OrdersWithFiltersForCountSpecification(OrderSpecParams orderParams) : base(o => 
        (!orderParams.FromDate.HasValue || o.OrderDate >= orderParams.FromDate) &&
        (!orderParams.ToDate.HasValue || o.OrderDate <= orderParams.ToDate) &&
        (string.IsNullOrEmpty(orderParams.Status) || o.Status == Enum.Parse<OrderStatus>(orderParams.Status))
        && (string.IsNullOrEmpty(orderParams.BuyerEmail) || o.BuyerEmail == orderParams.BuyerEmail))
        {
        }
    }
}