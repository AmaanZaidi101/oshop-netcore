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

    public class ProductController : ControllerBase
    {
        private readonly IOshopRepository<Product> _repository;
        private readonly IMapper _mapper;
        public ProductController(IOshopRepository<Product> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;

        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repository.GetAll();
            var productsDto = _mapper.Map<List<ProductDto>>(products);
            return Ok(productsDto);
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedProducts([FromQuery] PagingParams pagingParams)
        {
            Expression<Func<Product, bool>> expression = x => (x.CategoryId == pagingParams.CategoryId || 
                    string.IsNullOrWhiteSpace(pagingParams.CategoryId)) &&
                    (x.Id == pagingParams.ProductId || string.IsNullOrWhiteSpace(pagingParams.ProductId)) &&
                    (x.SoldOut ==  pagingParams.soldOut || pagingParams.soldOut == null);
            var products = await _repository.GetWithPaging(expression, pagingParams);
            var productsDto = _mapper.Map<List<ProductDto>>(products);
            Response.AddPagination(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages);
            return Ok(productsDto);
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<IActionResult> GetProductById(string id)
        {
            Expression<Func<Product, bool>> expression =  x => x.Id == id;
            var product = await _repository.GetFirst(expression);
            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        [Authorize(Policy = "RequireCustomerRole")]
        [HttpGet("category/{id}", Name = "GetProductByCategory")]
        public async Task<IActionResult> GetProductByCategory(string id)
        {
            Expression<Func<Product, bool>> expression =  x => x.CategoryId == id;
            var products = await _repository.GetByCondition(expression);
            var productsDto = _mapper.Map<List<ProductDto>>(products);
            return Ok(productsDto);
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            
            _repository.Insert(product);
            
            if(await _repository.SaveAll())
            {
                return CreatedAtRoute("GetProductById", new {id = product.Id}, productDto);
            }

            return BadRequest("Could not add product");
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            // Expression<Func<Product, bool>> expression = x => x.Id == productDto.Id;
            // var product = await _repository.GetFirst(expression);

            // if(product == null)
            //     return BadRequest("Product not found");

            // product.CategoryId = productDto.CategoryId;
            // product.Name = productDto.Name;
            // product.Price = productDto.Price;
            // product.ImageUrl = productDto.ImageUrl;
            // product.SoldOut = productDto.SoldOut;
            // product.Weight = productDto.Weight;

            var product = _mapper.Map<Product>(productDto);
            _repository.Update(product);
            
            if(await _repository.SaveAll())
            {
                return Ok(productDto);
            }

            return BadRequest("Could not update product");
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            Expression<Func<Product, bool>> expression = x => x.Id == id;
            var product = await _repository.GetFirst(expression);
            
            _repository.Delete(product);
            
            if(await _repository.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not delete product");
        }
    }
}