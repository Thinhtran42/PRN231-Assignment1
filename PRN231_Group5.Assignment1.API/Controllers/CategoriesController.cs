using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_Group5.Assignment1.Repo.Interfaces;
using PRN231_Group5.Assignment1.Repo.Models;
using PRN231_Group5.Assignment1.Repo.VIewModels.Category;

namespace PRN231_Group5.Assignment1.API.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAsync();
            if (categories == null || !categories.Any())
            {
                return NotFound();
            }
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIDAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] CreateCategoryViewModel categoryViewModel)
        {
            var category = _mapper.Map<Category>(categoryViewModel);

            Random random = new Random();
            category.CategoryId = random.Next();

            _unitOfWork.CategoryRepository.Insert(category);
            _unitOfWork.Save();

            return CreatedAtAction("GetCategory", new { id = category.CategoryId }, categoryViewModel);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryViewModel categoryViewModel)
        {
            var existedCategory = await _unitOfWork.CategoryRepository.GetByIDAsync(id);

            if (existedCategory is null)
            {
                return NotFound();
            }

            var updateCategory = _mapper.Map(categoryViewModel, existedCategory);

            _unitOfWork.CategoryRepository.Update(updateCategory);
            _unitOfWork.Save();

            return NoContent();
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var existedCategory = await _unitOfWork.CategoryRepository.GetByIDAsync(id);
            if (existedCategory is null)
            {
                return NotFound();
            }
            _unitOfWork.CategoryRepository.Delete(existedCategory);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
