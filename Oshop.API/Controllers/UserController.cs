using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Oshop.API.Data;
using Oshop.API.Dtos;
using Oshop.API.Helpers;
using Oshop.API.Models;

namespace Oshop.API.Controllers
{
    [Authorize(Policy = "RequireModeratorRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOshopRepository<User> _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public UserController(IOshopRepository<User> repository, IMapper mapper, UserManager<User> userManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _repository = repository;

        }

        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return BadRequest("Could not find user");

            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedUsers(PagingParams pagingParams)
        {
            var users = await _repository.GetAllWithPaging(pagingParams);

            if(users == null || users.Count == 0)
                return BadRequest("No Users Found");

            var usersToReturn = _mapper.Map<List<UserForDetailedDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        


    }
}