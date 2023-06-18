using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Data.Specifications;
using API.Interfaces;
using API.Models;
using API.Models.OrderAggregate;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo, IMapper mapper, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _basketRepo = basketRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Models.OrderAggregate.Address shippingAddress)
    {
            var basket = await _basketRepo.GetBasketAsync(basketId);

            var user = _userManager.Users.FirstOrDefault(u => u.Email == buyerEmail);
            
            var items = new List<OrderItem>();
            
            foreach (var item in basket.Items)
            {
                var productSpec = new ProductsWithTypesSpecification(item.Id);

                var productItem = await _unitOfWork.Repository<Product>().GetEntityWithSpec(productSpec);

                if (productItem == null) continue;
                
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.Photos?.Where(p => p.IsMain).FirstOrDefault()?.Url);
                
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                
                items.Add(orderItem);
            }
            
            // get delivery method from repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // calc subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            // check to see if order exists
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (order != null)
            {
                order.ShipToAddress = shippingAddress;
                order.DeliveryMethod = deliveryMethod;
                order.Subtotal = subtotal;

                _unitOfWork.Repository<Order>().Update(order);
            }
            else 
            {
                // create order
                order = new Order(items, user, shippingAddress, deliveryMethod, 
                    subtotal, basket.PaymentIntentId);

                _unitOfWork.Repository<Order>().Add(order);
            }

            // save to db
            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;
            
            // return order
            return order;
        }

        public async Task<int> GetOrdersCountAsync(OrderSpecParams orderParams)
        {
            var spec = new OrdersWithFiltersForCountSpecification(orderParams);

            return await _unitOfWork.Repository<Order>().CountAsync(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForAdminAsync(OrderSpecParams orderParams)
        {
            List<Expression<Func<Order, bool>>> criteria = new List<Expression<Func<Order, bool>>>();
                
            criteria.Add(o => (!orderParams.FromDate.HasValue || o.OrderDate >= orderParams.FromDate));
            criteria.Add(o => (!orderParams.ToDate.HasValue || o.OrderDate <= orderParams.ToDate));
            criteria.Add(o => (string.IsNullOrEmpty(orderParams.Status) || o.Status == Enum.Parse<OrderStatus>(orderParams.Status)));
            criteria.Add(o => (string.IsNullOrEmpty(orderParams.BuyerEmail) || o.BuyerEmail == orderParams.BuyerEmail));
            
            var spec = new OrdersWithItemsAndOrderingSpecificationForAdmin(orderParams, criteria);

            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }

        public async Task<Order> GetOrderForAdminByIdAsync(int id)
        {
            var spec = new OrdersWithItemsAndOrderingSpecificationForAdmin(id);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(OrderSpecParams orderParams)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(orderParams);

            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }
    }
}