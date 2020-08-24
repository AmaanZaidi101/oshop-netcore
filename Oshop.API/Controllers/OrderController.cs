using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Oshop.API.Data;
using Oshop.API.Dtos;
using Oshop.API.Helpers;
using Oshop.API.Interfaces;
using Oshop.API.Models;

namespace Oshop.API.Controllers
{
    [Route("api/order/{userId}")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        List<string> statuses = new List<string>
        {
            "pending",
            "confirmed",
            "delivered",
            "rejected"
        };
        private readonly IOshopRepository<Order> _repository;
        private readonly IOshopRepository<User> _userRepo;
        private readonly IMapper _mapper;
        private readonly ICartService _cartService;
        private readonly UserManager<User> _userManager;

        public OrderController(IOshopRepository<Order> repository, IOshopRepository<User> userRepo,
        IMapper mapper, ICartService cartService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _repository = repository;
            _userRepo = userRepo;
            _mapper = mapper;
            _cartService = cartService;
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllOrders([FromQuery] PagingParams pagingParams)
        {
            if (string.IsNullOrWhiteSpace(pagingParams.Status))
                pagingParams.Status = "pending";
            Expression<Func<Order, bool>> expression = x => (x.Status == pagingParams.Status || string.IsNullOrWhiteSpace(pagingParams.Status))
                                                        && (pagingParams.OrderId == x.Id || string.IsNullOrWhiteSpace(pagingParams.OrderId));
            var orders = await _repository.GetWithPaging(expression, pagingParams);
            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            Response.AddPagination(orders.CurrentPage, orders.PageSize, orders.TotalCount, orders.TotalPages);
            return Ok(ordersDto);
        }

        [HttpGet(Name = "GetOrders")]
        public async Task<IActionResult> GetOrders(string userId, [FromQuery] PagingParams pagingParams)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();

            Expression<Func<Order, bool>> expression = x => x.UserId == userId;

            var orders = await _repository.GetWithPaging(expression, pagingParams);

            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            Response.AddPagination(orders.CurrentPage, orders.PageSize, orders.TotalCount, orders.TotalPages);
            return Ok(ordersDto);

        }

        [Authorize(Policy = "RequireCustomerRole")]
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedOrders(string userId, [FromQuery] PagingParams pagingParams)
        {
            pagingParams.Status = pagingParams.Status != null ? pagingParams.Status : "pending";
            Expression<Func<Order, bool>> expression = x => x.UserId == userId && x.Status == pagingParams.Status
                                        && (x.Id == pagingParams.OrderId || string.IsNullOrWhiteSpace(pagingParams.OrderId));
            var orders = await _repository.GetWithPaging(expression, pagingParams);
            var ordersDto = _mapper.Map<List<OrderDto>>(orders);
            Response.AddPagination(orders.CurrentPage, orders.PageSize, orders.TotalCount, orders.TotalPages);
            return Ok(ordersDto);
        }

        [HttpGet("{orderId}", Name = "GetOrder")]
        public async Task<IActionResult> GetOrders(string userId, string orderId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);

            if (!userRoles.Contains("Admin") && !userRoles.Contains("Moderator") && userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();

            Expression<Func<Order, bool>> expression = x => x.UserId == userId
                                            && x.Id == orderId;

            if(userRoles.Contains("Admin") || userRoles.Contains("Moderator"))
                expression = x => x.Id == orderId;

            var order = await _repository.GetFirst(expression);

            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);

        }

        [Authorize(Policy = "RequireCustomerRole")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(string userId, OrderDto orderDto)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();

            Expression<Func<User, bool>> expression = x => x.Id == userId;

            var user = await _userRepo.GetFirst(expression);

            if (!string.IsNullOrWhiteSpace(orderDto.OrderShipping.Id))
            {
                orderDto.OrderShipping.Id = null;
            }

            orderDto.DatePlaced = DateTime.Now;
            orderDto.Status = "pending";

            var order = _mapper.Map<Order>(orderDto);

            user.Orders.Add(order);

            if (await _repository.SaveAll())
            {
                orderDto = _mapper.Map<OrderDto>(order);
                await _cartService.ClearCart(userId);
                return CreatedAtRoute("GetOrder", new { orderId = orderDto.Id }, orderDto);
            }

            return BadRequest("Could not create order");

        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPut("{orderId}/{status}")]
        public async Task<IActionResult> UpdateOrder(string userId, string orderId, string status)
        {

            if (!statuses.Contains(status))
            {
                return BadRequest("Unmatched status value");
            }

            Expression<Func<Order, bool>> expression = x => x.Id == orderId;

            var order = await _repository.GetFirst(expression);

            if (order == null)
                return BadRequest("Could not find order");

            order.Status = status;
            _repository.Update(order);

            if (await _repository.SaveAll())
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                return Ok(orderDto);
            }

            return BadRequest("Could not update Order");

        }

        [Authorize(Policy = "RequireCustomerRole")]
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(string userId, string orderId)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();

            Expression<Func<Order, bool>> expression = x => x.Id == orderId & x.UserId == userId;

            var order = await _repository.GetFirst(expression);

            if (order == null)
                return BadRequest("Could not find order");

            if(order.Status == "delivered")
                return BadRequest("Order already delivered");

            _repository.Delete(order);

            if (await _repository.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not update Order");

        }
    }
}