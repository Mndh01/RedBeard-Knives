using System.Threading.Tasks;
using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models.OrderAggregate;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using API.Data.Specifications;
using API.Helpers;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = User.GetEmail();

            var address = _mapper.Map<AddressDto, Models.OrderAggregate.Address>(orderDto.ShipToAddress);
            
            var order = await _orderService.CreateOrderAsync(email, 
                orderDto.DeliveryMethodId, orderDto.BasketId, address);
            
            if (order == null) return BadRequest("Problem creating order");
            
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser([FromQuery] OrderSpecParams orderParams)
        {
            var email = User.GetEmail();

            orderParams.BuyerEmail = email;

            var orders = await _orderService.GetOrdersForUserAsync(orderParams);

            var totalOrders = await _orderService.GetOrdersCountAsync(orderParams);

            return Ok(new Pagination<OrderToReturnDto>(orderParams.PageIndex, orderParams.PageSize, totalOrders, _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var email = User.GetEmail();

            var order = await _orderService.GetOrderByIdAsync(id, email);

            if (order == null) return NotFound();

            return _mapper.Map<Order, OrderToReturnDto>(order);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }
    }
}