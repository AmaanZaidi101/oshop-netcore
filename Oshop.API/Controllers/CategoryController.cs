using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oshop.API.Data;
using Oshop.API.Dtos;
using Oshop.API.Helpers;
using Oshop.API.Models;

namespace Oshop.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly IOshopRepository<Category> _repository;
        private readonly IMapper _mapper;
        public CategoryController(IOshopRepository<Category> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _repository.GetAll();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
            return Ok(categoriesDto);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedCategories([FromQuery] PagingParams pagingParams)
        {
            var categories = await _repository.GetAllWithPaging(pagingParams);
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
            Response.AddPagination(categories.CurrentPage, categories.PageSize, categories.TotalCount, categories.TotalPages);
            return Ok(categoriesDto);
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            Expression<Func<Category, bool>> expression =  x => x.Id == id;
            var category = await _repository.GetFirst(expression);
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            
            _repository.Insert(category);
            
            
            if(await _repository.SaveAll())
            {
                return CreatedAtRoute("GetCategoryById", new {id = category.Id}, categoryDto);
            }

            return BadRequest("Could not add category");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            
            _repository.Update(category);
            
            if(await _repository.SaveAll())
            {
                return Ok(categoryDto);
            }

            return BadRequest("Could not update category");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            Expression<Func<Category, bool>> expression = x => x.Id == id;
            var category = await _repository.GetFirst(expression);
            
            _repository.Delete(category);
            
            if(await _repository.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not delete category");
        }

    }
}