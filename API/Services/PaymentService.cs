using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Specifications;
using API.Interfaces;
using API.Models;
using API.Models.OrderAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = API.Models.Product;

namespace API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        private readonly UserManager<AppUser> _userManager;
        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration config, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<List<PaymentIntent>> GetUserPayments(string customerId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            var options = new PaymentIntentListOptions
            {
                Customer = customerId
            };

            var service = new PaymentIntentService();

            var paymentIntents = await service.ListAsync(options);

            return paymentIntents.Data;

        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);

            if(basket == null) return null;
            
            var shippingPrice = 0m;

            if(basket.DeliveryMethodId.HasValue) 
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync((int)basket.DeliveryMethodId);

                shippingPrice = deliveryMethod.Price;
            }

            foreach(var item in basket.Items) 
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                if(item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            var service = new PaymentIntentService();

            PaymentIntent intent;

            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long) basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long) (shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> {"card"},
                    // Customer = userId.ToString(),
                };

                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long) basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long) (shippingPrice * 100)
                };

                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if(order == null) return null;

            order.Status = OrderStatus.PaymentFailed;

            await _unitOfWork.Complete();

            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if(order == null) return null;

            order.Status = OrderStatus.PaymentReceived;

            foreach(OrderItem item in order.OrderItems) 
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ItemOrdered.ProductItemId);

                productItem.SoldItems += item.Quantity;
                productItem.InStock -= item.Quantity;
            }

            await _unitOfWork.Complete();

            return order;
        }
    }
}