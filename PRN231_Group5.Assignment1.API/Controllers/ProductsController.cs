﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_Group5.Assignment1.Repo.Interfaces;
using PRN231_Group5.Assignment1.Repo.Models;
using PRN231_Group5.Assignment1.Repo.VIewModels.Product;

namespace PRN231_Group5.Assignment1.API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/products
        [HttpGet()]
        public async Task<IActionResult> GetProducts(
                                                [FromQuery] int? pageIndex,
                                                [FromQuery] string? productName,
                                                [FromQuery] decimal? minUnitPrice,
                                                [FromQuery] decimal? maxUnitPrice,
                                                [FromQuery] int? categoryId,
                                                [FromQuery] string? sortBy)
        {
            int pageSize = 4;
            var products = _unitOfWork.ProductRepository.Get();
            if (!string.IsNullOrEmpty(productName))
            {
                products = products.Where(p => p.ProductName.Contains(productName));
            }

            if (minUnitPrice.HasValue)
            {
                products = products.Where(p => p.UnitPrice >= minUnitPrice.Value);
            }

            if (maxUnitPrice.HasValue)
            {
                products = products.Where(p => p.UnitPrice <= maxUnitPrice.Value);
            }

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == "desc")
                {
                    products = products.OrderByDescending(p => p.UnitPrice);
                }
                else
                {
                    products = products.OrderBy(p => p.UnitPrice);
                }
            }
            //products = _unitOfWork.ProductRepository.Get(pageIndex: pageIndex.HasValue ? Math.Max(pageIndex.Value, 1) : 1, pageSize: pageSize, includeProperties: "Category");
            // Thực hiện tìm kiếm và lưu trữ kết quả vào biến tempProducts
            var tempProducts = products.ToList();

            var paginatedProducts = tempProducts.Skip((pageIndex ?? 0) * pageSize).Take(pageSize);


            return Ok(paginatedProducts);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIDAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductViewModel productViewModel)
        {
            var product = _mapper.Map<Product>(productViewModel);

            Random random = new Random();
            product.ProductId = random.Next();

            var category =
                await _unitOfWork.CategoryRepository.GetAsync(c => c.CategoryId == productViewModel.CategoryId);
            if (!category.Any())
            {
                return BadRequest($"Not have category with id {productViewModel.CategoryId}");
            }

            _unitOfWork.ProductRepository.Insert(product);
            _unitOfWork.Save();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, productViewModel);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductViewModel productViewModel)
        {
            var existedProduct = await _unitOfWork.ProductRepository.GetByIDAsync(id);

            if (existedProduct is null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.CategoryRepository.GetAsync(c => c.CategoryId == productViewModel.CategoryId);
            if (!category.Any())
            {
                return BadRequest($"Not have category with id {productViewModel.CategoryId}");
            }

            var updateProduct = _mapper.Map(productViewModel, existedProduct);

            _unitOfWork.ProductRepository.Update(updateProduct);
            _unitOfWork.Save();

            return NoContent();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existedProduct = await _unitOfWork.ProductRepository.GetByIDAsync(id);
            if (existedProduct is null)
            {
                return NotFound();
            }
            _unitOfWork.ProductRepository.Delete(existedProduct);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
