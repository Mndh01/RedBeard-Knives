using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Specifications;
using API.Interfaces;
using API.Models;
using API.Models.OrderAggregate;
using AutoMapper;

namespace API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _basketRepo = basketRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Models.OrderAggregate.Address shippingAddress)
        {
            var basket = await _basketRepo.GetBasketAsync(basketId);
            
            var items = new List<OrderItem>();
            
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                if (productItem == null) continue;
                
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.Photos?.Where(p => p.IsMain).FirstOrDefault().Url);
                
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                
                items.Add(orderItem);
            }
            
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var subtotal = items.Sum(item => item.Price * item.Quantity);

            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);

            _unitOfWork.Repository<Order>().Add(order);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            await _basketRepo.DeleteBasketAsync(basketId);
            
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersAsync()
        {
            return await _unitOfWork.Repository<Order>().ListAllAsync();
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}