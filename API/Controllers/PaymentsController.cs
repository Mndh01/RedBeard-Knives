using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using API.Models;
using API.Models.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private const string WhSecret = "whsec_b2cc8deb971cff64686f3826cfd5bb9e7e0b6d02f624cf4e74301af9d701c6b1";
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private readonly UserManager<AppUser> _userMangaer;
        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger, UserManager<AppUser> userMangaer)
        {
            _userMangaer = userMangaer;
            _logger = logger;
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpGet("user-payments")]
        public async Task<ActionResult<ICollection<PaymentIntent>>> GetUserPayments()
        {
            var user = await _userMangaer.FindByEmailAsync(User.GetEmail());

            var payments = await _paymentService.GetUserPayments(user.StripeCustomerId);

            return Ok(payments);
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null) return BadRequest("Problem with your basket");

            return basket;
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook() 
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WhSecret);

            PaymentIntent Intent;
            Order order;
            
            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    Intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment succeeded: ", Intent.Id);
                    order = await _paymentService.UpdateOrderPaymentSucceeded(Intent.Id);
                    _logger.LogInformation("Order updated to payment received: ", order.Id);
                    break;
                case "payment_intent.payment_failed":
                    Intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment failed: ", Intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(Intent.Id);
                    _logger.LogInformation("Order updated to payment failed: ", order.Id);
                    break;
            }

            return new EmptyResult();
        }
    }
}