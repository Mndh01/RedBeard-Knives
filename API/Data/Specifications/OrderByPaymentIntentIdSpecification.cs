using API.Models.OrderAggregate;

namespace API.Data.Specifications
{
    public class OrderByPaymentIntentIdSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentIntentId) 
            : base(o => o.PaymentIntentId == paymentIntentId)
        {
            AddInclude(o => o.OrderItems);
        }
    }
}