using API.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SizesController : ControllerBase
    {
        private readonly ISizeService _sizeService;

        public SizesController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        // Отримати всі типи розмірів
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<SizeTypeDto>>> GetAllSizeTypes()
        {
            var sizeTypes = await _sizeService.GetAllSizeTypesAsync();
            var sizeTypeDtos = sizeTypes.Select(st => new SizeTypeDto
            {
                Id = st.Id,
                Name = st.Name,
                Sizes = st.Sizes.Select(s => new SizeDto
                {
                    Id = s.Id,
                    Value = s.Value,
                    IsAvailable = s.IsAvailable
                }).ToList()
            });

            return Ok(sizeTypeDtos);
        }

        // Отримати тип розміру за ідентифікатором
        [HttpGet("types/{id}")]
        public async Task<ActionResult<SizeTypeDto>> GetSizeTypeById(string id)
        {
            var sizeType = await _sizeService.GetSizeTypeByIdAsync(id);
            if (sizeType == null)
                return NotFound($"SizeType with ID {id} not found.");

            var sizeTypeDto = new SizeTypeDto
            {
                Id = sizeType.Id,
                Name = sizeType.Name,
                Sizes = sizeType.Sizes.Select(s => new SizeDto
                {
                    Id = s.Id,
                    Value = s.Value,
                    IsAvailable = s.IsAvailable
                }).ToList()
            };

            return Ok(sizeTypeDto);
        }

        // Додати новий тип розміру разом із розмірами
        [HttpPost("types")]
        public async Task<ActionResult> AddSizeType([FromBody] CreateSizeTypeDto createSizeTypeDto)
        {
            // Створення нового типу розміру (SizeType)
            var sizeType = new SizeType
            {
                Name = createSizeTypeDto.Name
            };

            // Додавання нового SizeType
            await _sizeService.AddSizeTypeAsync(sizeType);

            // Присвоєння кожному Size TypeId, щоб вони були пов'язані з новим SizeType
            foreach (var sizeDto in createSizeTypeDto.Sizes)
            {
                var size = new Size
                {
                    Value = sizeDto.Value,
                    IsAvailable = sizeDto.IsAvailable,
                    SizeTypeId = sizeType.Id // прив'язуємо Size до нового SizeType
                };

                // Додавання кожного розміру в базу даних
                await _sizeService.AddSizeAsync(size);
            }

            // Повертаємо статус створення нового SizeType разом з доданими Size
            return CreatedAtAction(nameof(GetSizeTypeById), new { id = sizeType.Id }, sizeType);
        }


        // Оновити тип розміру
        /*[HttpPut("types/{id}")]
        public async Task<ActionResult> UpdateSizeType(string id, [FromBody] UpdateSizeTypeDto updateSizeTypeDto)
        {
            var existingType = await _sizeService.GetSizeTypeByIdAsync(id);
            if (existingType == null)
                return NotFound($"SizeType with ID {id} not found.");

            existingType.Name = updateSizeTypeDto.Name;

            // Оновлюємо розміри, якщо вони були передані
            if (updateSizeTypeDto.Sizes != null && updateSizeTypeDto.Sizes.Any())
            {
                existingType.Sizes = updateSizeTypeDto.Sizes.Select(s => new Size
                {
                    Id = s.Id,
                    Value = s.Value,
                    IsAvailable = s.IsAvailable
                }).ToList();
            }

            await _sizeService.UpdateSizeTypeAsync(existingType);
            return NoContent();
        }*/

        // Видалити тип розміру
        [HttpDelete("types/{id}")]
        public async Task<ActionResult> DeleteSizeType(string id)
        {
            var existingType = await _sizeService.GetSizeTypeByIdAsync(id);
            if (existingType == null)
                return NotFound($"SizeType with ID {id} not found.");

            await _sizeService.DeleteSizeTypeAsync(id);
            return Ok();
        }

        // Додати новий розмір
        [HttpPost]
        public async Task<ActionResult> AddSize([FromBody] AddNewSizeDto createSizeDto)
        {
            var size = new Size
            {
                Value = createSizeDto.Value,
                IsAvailable = createSizeDto.IsAvailable,
                SizeTypeId = createSizeDto.SizeTypeId
            };

            await _sizeService.AddSizeAsync(size);

            return CreatedAtAction(nameof(GetSizeById), new { id = size.Id }, size);
        }

        // Отримати розмір за ідентифікатором
        [HttpGet("{id}")]
        public async Task<ActionResult<SizeDto>> GetSizeById(string id)
        {
            var size = await _sizeService.GetSizeByIdAsync(id);
            if (size == null)
                return NotFound($"Size with ID {id} not found.");

            var sizeDto = new SizeDto
            {
                Id = size.Id,
                Value = size.Value,
                IsAvailable = size.IsAvailable
            };

            return Ok(sizeDto);
        }

        // Оновити розмір
        /*[HttpPut("{id}")]
        public async Task<ActionResult> UpdateSize(string id, [FromBody] UpdateSizeDto updateSizeDto)
        {
            var existingSize = await _sizeService.GetSizeByIdAsync(id);
            if (existingSize == null)
                return NotFound($"Size with ID {id} not found.");



            existingSize.Value = updateSizeDto.Value;
            existingSize.IsAvailable = updateSizeDto.IsAvailable;

            await _sizeService.UpdateSizeAsync(existingSize);
            return NoContent();
        }*/

        // Видалити розмір
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSize(string id)
        {
            var size = await _sizeService.GetSizeByIdAsync(id);
            if (size == null)
                return NotFound($"Size with ID {id} not found.");

            await _sizeService.DeleteSizeAsync(id);
            return Ok();
        }

        // Отримати всі розміри за ідентифікатором типу розміру
        [HttpGet("types/{sizeTypeId}/sizes")]
        public async Task<ActionResult<IEnumerable<SizeDto>>> GetSizesBySizeTypeId(string sizeTypeId)
        {
            var sizes = await _sizeService.GetSizesBySizeTypeIdAsync(sizeTypeId);

            var sizeDtos = sizes.Select(s => new SizeDto
            {
                Id = s.Id,
                Value = s.Value,
                IsAvailable = s.IsAvailable
            });

            return Ok(sizeDtos);
        }
    }
}
