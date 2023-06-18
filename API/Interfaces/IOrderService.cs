using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data.Specifications;
using API.Models.OrderAggregate;

namespace API.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId, Address shippingAddress);
        Task<IReadOnlyList<Order>> GetOrdersForAdminAsync(OrderSpecParams orderParams);
        Task<Order> GetOrderForAdminByIdAsync(int id);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(OrderSpecParams orderParams);
        Task<Order> GetOrderByIdAsync(int id, string buyerEmail);
        Task<int> GetOrdersCountAsync(OrderSpecParams orderParams);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}