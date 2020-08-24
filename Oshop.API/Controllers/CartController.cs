using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oshop.API.Data;
using Oshop.API.Dtos;
using Oshop.API.Models;

namespace Oshop.API.Controllers
{
    [Authorize(Policy = "RequireCustomerRole")]
    [Route("api/cart/{userId}")]
    [ApiController]
    public class CartController: ControllerBase
    {
        private readonly IOshopRepository<Cart> _repository;
        private readonly IMapper _mapper;
        public CartController(IOshopRepository<Cart> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }

        [HttpGet(Name = "GetCart")]
        public async Task<IActionResult> GetCart(string userId)
        {
            if(userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();
            
            Expression<Func<Cart, bool>> expression = x => x.UserId == userId;
            var cart = await _repository.GetFirst(expression);
            var cartDto = _mapper.Map<CartDto>(cart);
            return Ok(cartDto);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddToCart(string userId, ProductDto productDto)
        {
            if(userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();

            Expression<Func<Cart, bool>> expression = x => x.UserId == userId;
            var cart = await _repository.GetFirst(expression);

            if(cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };
                var product = _mapper.Map<Product>(productDto);
                var cartItem = new CartItem
                    {
                        Quantity = 1,
                        ProductId = product.Id,
                        TotalPrice = product.Price
                    };
                _repository.Insert(cart);
                if(await _repository.SaveAll())
                {
                    cart.CartItems = new List<CartItem>();
                    cart.CartItems.Add(cartItem);
                }
            }
            else
            {
                var cartItem = cart.CartItems?.FirstOrDefault(x => x.Product.Id == productDto.Id);

                if (cartItem != null)
                {
                    cartItem.Quantity++;
                    cartItem.TotalPrice = cartItem.Product.Price * cartItem.Quantity;
                }
                else
                {
                    var product = _mapper.Map<Product>(productDto);
                    cartItem = new CartItem
                    {
                        Quantity = 1,
                        ProductId = product.Id,
                        TotalPrice = product.Price
                    };
                }
                    
                cart.CartItems.Add(cartItem);
            }

            if(await _repository.SaveAll())
            {
                var cartDto = _mapper.Map<CartDto>(cart);
                var index = cartDto.CartItems.FindIndex(x => x.ProductId == productDto.Id);
                if (cartDto.CartItems.ElementAt(index)?.Product == null)
                {
                    cartDto.CartItems.ElementAt(index).Product = productDto;
                }
                return CreatedAtRoute("GetCart", new {id = cartDto.Id}, cartDto);
            }

            return BadRequest("Could not add product to cart");
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> RemoveFromCart(string userId, ProductDto productDto)
        {
            if(userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();

            Expression<Func<Cart, bool>> expression = x => x.UserId == userId;
            var cart = await _repository.GetFirst(expression);
            
            if(cart == null)
            {
                return BadRequest("Cart is Empty");
            }
            else
            {
                var product = _mapper.Map<Product>(productDto);
                var cartItem = cart.CartItems?.FirstOrDefault(x => x.Product.Id == productDto.Id);

                if (cartItem != null)
                {
                    cartItem.Quantity--;
                    cartItem.TotalPrice = cartItem.Product.Price * cartItem.Quantity;
                }
                else
                {
                    return BadRequest("Product not found in cart");
                }

                if (cartItem.Quantity == 0)
                    cart.CartItems.Remove(cartItem);
            }

            if(await _repository.SaveAll())
            {
                var cartDto = _mapper.Map<CartDto>(cart);
                return CreatedAtRoute("GetCart", new {id = cartDto.Id}, cartDto);
            }

            return BadRequest("Could not remove product from cart");
            
        }

        [HttpPost("ClearCart")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            if(userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();

            Expression<Func<Cart, bool>> expression = x => x.UserId == userId;
            var cart = await _repository.GetFirst(expression);
            
            if(cart == null)
            {
                return BadRequest("Cart is Empty");
            }
            else
            {
                cart.CartItems.Clear();
            }

            if(await _repository.SaveAll())
            {
                var cartDto = _mapper.Map<CartDto>(cart);
                return CreatedAtRoute("GetCart", new {id = cartDto.Id}, cartDto);
            }

            return BadRequest("Could not clear cart");
        }

    }
}