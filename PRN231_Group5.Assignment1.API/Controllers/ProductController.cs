using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_Group5.Assignment1.Repo.Interfaces;
using PRN231_Group5.Assignment1.Repo.Models;
using PRN231_Group5.Assignment1.Repo.VIewModels.Product;

namespace PRN231_Group5.Assignment1.API.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        // GET: api/product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _unitOfWork.ProductRepository.GetAsync();
            if (products == null || !products.Any())
            {
                return NotFound();
            }
            return Ok(products);
        }

        // GET: api/product/5
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

        // POST: api/Product
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

        // PUT: api/product/5
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

        // DELETE: api/Product/5
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
