using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Oshop.API.Data;
using Oshop.API.Dtos;
using Oshop.API.Interfaces;
using Oshop.API.Models;

namespace Oshop.API.Services
{
    public class CartService : ICartService
    {
        private readonly IOshopRepository<Cart> _repository;
        private readonly IMapper _mapper;
        private readonly IOshopRepository<CartItem> _cartItemRepo;
        public CartService(IOshopRepository<Cart> repository, IOshopRepository<CartItem> cartItemRepo, IMapper mapper)
        {
            _cartItemRepo = cartItemRepo;
            _mapper = mapper;
            _repository = repository;

        }
        public async Task<Response> ClearCart(string userId)
        {

            Expression<Func<Cart, bool>> expression = x => x.UserId == userId;
            var cart = await _repository.GetFirst(expression);

            if (cart == null)
            {
                var response = new Response()
                {
                    Message = "Cart is Empty",
                    StatusCode = HttpStatusCode.BadRequest
                };

                return response;
            }
            else
            {
                var cartItems = await _cartItemRepo.GetByCondition(x => x.CartId == cart.Id);
                cart.CartItems.Clear();
                foreach (var cartItem in cartItems)
                {
                    _cartItemRepo.Delete(cartItem);
                }
            }

            if (await _repository.SaveAll())
            {
                var cartDto = _mapper.Map<CartDto>(cart);
                return new Response()
                {
                    StatusCode = HttpStatusCode.Created,
                    Result = cartDto,
                    Message = "GetCart"

                };
            }

            return new Response()
            {
                Message = "Could not clear cart",
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        public async Task<Response> GetCart(string userId)
        {
            Expression<Func<Cart, bool>> expression = x => x.UserId == userId;
            var cart = await _repository.GetFirst(expression);
            var cartDto = _mapper.Map<CartDto>(cart);
            var response = new Response()
            {
                StatusCode = HttpStatusCode.OK
            };
            return response;
        }

        public async Task<Response> AddToCart(string userId, ProductDto productDto)
        {   
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
                return new Response()
                {
                    StatusCode = HttpStatusCode.Created,
                    Message = "GetCart",
                    Result = new {id = cartDto.Id}
                };
            }

            return new Response()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Could not add product to cart"
            };
        }

        public async Task<Response> RemoveFromCart(string userId, ProductDto productDto)
        {
            Expression<Func<Cart, bool>> expression = x => x.UserId == userId;
            var cart = await _repository.GetFirst(expression);
            
            if(cart == null)
            {
                return new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Cart is Empty"
                };
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
                    return new Response()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Product not found in cart"
                    };
                }

                if (cartItem.Quantity == 0)
                    cart.CartItems.Remove(cartItem);
            }

            if(await _repository.SaveAll())
            {
                var cartDto = _mapper.Map<CartDto>(cart);
                return new Response()
                {
                    StatusCode = HttpStatusCode.Created,
                    Message = "GetCart",
                    Result = cartDto
                };
            }

            return new Response()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = "Could not remove product from cart", 
            };
            
        }
    }
}