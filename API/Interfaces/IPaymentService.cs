using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using API.Models.OrderAggregate;
using Stripe;

namespace API.Interfaces
{
    public interface IPaymentService
    {
        Task<List<PaymentIntent>> GetUserPayments(string customerId);
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
        Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId);
        Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}