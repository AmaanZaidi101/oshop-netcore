using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    [Authorize(Policy = "RequireAdminRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IOshopRepository<User> _repository;
        private readonly IMapper _mapper;
        public AdminController(IOshopRepository<User> repository, UserManager<User> userManager, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _userManager = userManager;
        }

        [HttpGet("WithRoles")]
        public async Task<IActionResult> GetUsersWithRoles([FromQuery]PagingParams pagingParams)
        {
            Expression<Func<User, bool>> expression = x => (x.Id == pagingParams.UserId || pagingParams.UserId == null)
                                    && (x.UserRoles.Any(y => y.Role.Name == pagingParams.Role) || pagingParams.Role == null)
                                    && (x.UserName == pagingParams.UserName || pagingParams.UserName == null);
            var users = await _repository.GetWithPaging(expression, pagingParams);

            if (users == null || users.Count == 0)
                return BadRequest("No Users Found");

            var usersToReturn = _mapper.Map<List<UserForRolesDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpPost("editRoles/{userId}")]
        public async Task<IActionResult> EditUserRoles(string userId, RolesDto rolesDto)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return BadRequest("User not found");

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = rolesDto.RoleNames;

            selectedRoles =  selectedRoles ?? new [] {"Customer"};
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                return BadRequest("Could not add roles to user");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to remove roles from user");

            return Ok(await _userManager.GetRolesAsync(user));
        }
    }
}