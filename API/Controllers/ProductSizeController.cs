using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductSizeController : ControllerBase
    {
        private readonly IProductSizeService _productSizeService;

        public ProductSizeController(IProductSizeService productSizeService)
        {
            _productSizeService = productSizeService;
        }

        [HttpGet("{productId}/{sizeId}")]
        public async Task<IActionResult> GetProductSize(string productId, string sizeId)
        {
            var productSize = await _productSizeService.GetProductSizeByIdAsync(productId, sizeId);
            if (productSize == null)
            {
                return NotFound();
            }

            return Ok(productSize);
        }

        [HttpPut("{productId}/{sizeId}")]
        public async Task<IActionResult> UpdateProductSizeAvailability(string productId, string sizeId, [FromBody] bool isAvailable)
        {
            await _productSizeService.UpdateProductSizeAvailabilityAsync(productId, sizeId, isAvailable);
            return NoContent();
        }
    }
}
