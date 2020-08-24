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
    [Route("api/shipping/{userId}")]
    [ApiController]
    public class ShippingController: ControllerBase
    {
        private readonly IOshopRepository<Shipping> _repository;
        private readonly IMapper _mapper;
        private readonly IOshopRepository<User> _userRepo;

        public ShippingController(IOshopRepository<Shipping> repository, IOshopRepository<User> userRepo, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        [HttpGet(Name = "GetShipping")]
        public async Task<IActionResult> GetShippingAddresses(string userId)
        {
            if(userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();

            Expression<Func<Shipping, bool>> expression = x => x.UserId == userId;

            var shippings = await _repository.GetByCondition(expression);
            var shippingsDto = _mapper.Map<List<ShippingDto>>(shippings);

            return Ok(shippingsDto);
        }

        [HttpGet("GetPreferred",Name = "GetPreferredShipping")]
        public async Task<IActionResult> GetPreferredShippingAddresses(string userId)
        {
            if(userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();

            Expression<Func<Shipping, bool>> expression = x => x.UserId == userId && x.IsPreferred;

            var shipping = await _repository.GetFirst(expression);
            var shippingDto = _mapper.Map<ShippingDto>(shipping);

            return Ok(shippingDto);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddShipping(string userId, ShippingDto shippingDto)
        {
            if(shippingDto.UserId == null)
                shippingDto.UserId = userId;

            if(userId != User.FindFirst(ClaimTypes.NameIdentifier).Value || userId != shippingDto.UserId)
                return Unauthorized();
            
            Expression<Func<User, bool>> expression = x => x.Id == userId;

            var user = await _userRepo.GetFirst(expression);

            var shipping = _mapper.Map<Shipping>(shippingDto);

            shippingDto.IsPreferred = shipping.IsPreferred = user.Shippings?.Count == 0 ? true : shipping.IsPreferred;

            if(shipping.IsPreferred)
            {
                foreach (var s in user.Shippings)
                {
                    s.IsPreferred = false;
                }
            }
            
            user.Shippings.Add(shipping);

            if(await _repository.SaveAll())
            {
                shippingDto = _mapper.Map<ShippingDto>(shipping);
                return CreatedAtRoute("GetShipping", new {id = shippingDto.Id}, shippingDto);
            }

            return BadRequest("Could not add shipping address");
        }

        [HttpPut("{shippingId}")]
        public async Task<IActionResult> UpdateShipping(string userId, string shippingId, ShippingDto shippingDto)
        {
            if(userId != User.FindFirst(ClaimTypes.NameIdentifier).Value || userId != shippingDto.UserId)
                return Unauthorized();

            
            Expression<Func<User, bool>> predicate = x => x.Id == userId;
            var user = await _userRepo.GetFirst(predicate);
            if(shippingDto.IsPreferred)
            {
                foreach (var s in user.Shippings)
                {
                    s.IsPreferred = false;
                }
            }
            
            Expression<Func<Shipping, bool>> expression = x => x.Id == shippingId && x.UserId == userId;
            var shipping = await _repository.GetFirst(expression);
            shipping.AddressLine1 = shippingDto.AddressLine1;
            shipping.AddressLine2 = shippingDto.AddressLine2;
            shipping.City = shippingDto.City;
            shipping.IsPreferred = shippingDto.IsPreferred;
            shipping.ShippingName = shippingDto.ShippingName;
            shipping.State = shippingDto.State;

            _repository.Update(shipping);

            if(await _repository.SaveAll())
            {
                shippingDto = _mapper.Map<ShippingDto>(shipping);
                return CreatedAtRoute("GetShipping", new {id = shippingDto.Id}, shippingDto);
            }

            return BadRequest("Could not update shipping");
        }
        
        [HttpDelete("{shippingId}")]
        public async Task<IActionResult> DeleteShipping(string userId, string shippingId)
        {
            if(userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return Unauthorized();

            Expression<Func<User, bool>> expression = x => x.Id == userId;
            
            var user = await _userRepo.GetFirst(expression);
            var shipping = user.Shippings.FirstOrDefault(x => x.Id == shippingId);
            
            if(shipping == null)
                return BadRequest("Could not find shipping");

            bool changePreferred = false;

            if(shipping.IsPreferred)
                changePreferred = true;

            user.Shippings.Remove(shipping);
            
            if(changePreferred)
            {
                var newPreferred = user.Shippings.FirstOrDefault();
                if(newPreferred != null)
                newPreferred.IsPreferred = true;
            }
            
            if(await _repository.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not delete shipping");
        }
    }
}