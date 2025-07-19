using ECommerce.Application.DTOs.AccountDTOs;
using ECommerce.Application.DTOs.SellerDTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        // POST: api/Seller/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] SellerCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _sellerService.AddAsync(dto);
                return Ok(new { Message = "Seller registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // POST: api/Seller/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await _sellerService.LoginAsync(dto);
            if (!result.IsSuccessful)
                return StatusCode(401, new { Message = result.Message });

            return Ok(result);
        }

        // GET: api/Seller
        [HttpGet]
        [Authorize(Roles = "Admin")] // Rol kontrolü opsiyoneldir
        public async Task<IActionResult> GetAll()
        {
            var sellers = await _sellerService.GetAllAsync();
            return Ok(sellers);
        }

        // GET: api/Seller/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var seller = await _sellerService.GetByIdAsync(id);
                return Ok(seller);
            }
            catch (Exception ex)
            {
                return NotFound(new { ex.Message });
            }
        }

        // PUT: api/Seller
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SellerUpdateDTO dto)
        {
            try
            {
                await _sellerService.UpdateAsync(dto);
                return Ok(new { Message = "Seller updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // DELETE: api/Seller/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.DeleteAsync(id);
                return Ok(new { Message = "Seller deleted successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(new { ex.Message });
            }
        }
    }
}

