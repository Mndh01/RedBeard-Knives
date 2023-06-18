using API.Models.OrderAggregate;

namespace API.Data.Specifications
{
    public class OrdersWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecification(OrderSpecParams orderParams) : base(o => o.BuyerEmail == orderParams.BuyerEmail)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);
            ApplyPaging((orderParams.PageIndex - 1) * orderParams.PageSize, orderParams.PageSize);
        }

        public OrdersWithItemsAndOrderingSpecification(int id, string email) : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}