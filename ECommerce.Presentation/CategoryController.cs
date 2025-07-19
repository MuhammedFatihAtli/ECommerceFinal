using ECommerce.Application.DTOs.CategoryDTOs;
using ECommerce.Application.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;


namespace ECommerce.Presentation
{
    [Route("api/[controller]s/")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IServiceUnit _service;

        public CategoryController(IServiceUnit service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var cats = await _service.CategoryService.GetAllAsync();
            return Ok(cats);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.CategoryService.AddAsync(model);
            return Ok();
        }
    }
}
