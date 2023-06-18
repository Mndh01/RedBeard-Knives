using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using API.Models.OrderAggregate;

namespace API.Data.Specifications
{
    public class OrdersWithItemsAndOrderingSpecificationForAdmin : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecificationForAdmin(OrderSpecParams orderParams, List<Expression<Func<Order, bool>>> criteria) : base(criteria)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
            ApplyPaging(orderParams.PageSize * (orderParams.PageIndex - 1), orderParams.PageSize);
        }

        public OrdersWithItemsAndOrderingSpecificationForAdmin(int id) : base(o => o.Id == id)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}