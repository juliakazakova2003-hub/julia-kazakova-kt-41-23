using JuliaKazakovaKt_41_23.Classes;
using JuliaKazakovaKt_41_23.Filters.DisciplineFilters;
using JuliaKazakovaKt_41_23.Interfaces.DisciplineInterfaces;
using JuliaKazakovaKt_41_23.Models;
using Microsoft.AspNetCore.Mvc;

namespace JuliaKazakovaKt_41_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DisciplinesController : ControllerBase
    {
        private readonly IDisciplineService _disciplineService;

        public DisciplinesController(IDisciplineService disciplineService)
        {
            _disciplineService = disciplineService;
        }

        [HttpPost("by_direction", Name = "GetDisciplinesByDirection")]
        public async Task<IActionResult> GetDisciplinesByDirectionAsync(
            [FromBody] DisciplineDirectionFilter filter,
            CancellationToken cancellationToken = default)
        {
            var disciplines =
                await _disciplineService.GetDisciplinesByDirectionAsync(
                    filter, cancellationToken);

            return Ok(disciplines);
        }

        /*[HttpPost("CreateDiscipline")]
        public async Task<IActionResult> CreateDisciplineAsync([FromBody] Discipline discipline,CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var disciplines = await _disciplineService.CreateDisciplineAsync(discipline, cancellationToken);
            return Ok(disciplines);
        }*/
        [HttpPost("CreateDiscipline")]
        public async Task<IActionResult> CreateDiscipline(DisciplineCreate dto,CancellationToken cancellationToken)
        {
            var discipline = new Discipline
            {
                Name = dto.Name,
                Direction = dto.Direction
            };

            var result = await _disciplineService.CreateDisciplineAsync(discipline, cancellationToken);

            if (result == null)
            {
                return Conflict("Такая дисциплина уже существует");
            }

            return Ok(result);
        }

        [HttpPut("UpdateDiscipline")]
        public async Task<IActionResult> UpdateDisciplineAsync([FromBody] Discipline disciplineNew,CancellationToken cancellationToken = default)
        {
            var disciplines = await _disciplineService.UpdateDisciplineAsync(
                disciplineNew, cancellationToken);

            if (disciplines == null)
            {
                return NotFound("Дисциплина не найдена");
            }
            return Ok(disciplines);
        }

        [HttpDelete("DeleteDiscipline")]
        public async Task<IActionResult> DeleteDisciplineAsync(int idDelete, CancellationToken cancellationToken = default)
        {
            var disciplineIsDel = await _disciplineService.DeleteDisciplineAsync(
                idDelete, cancellationToken);

            if (!disciplineIsDel)
            {
                return NotFound("Дисциплина не найдена");
            }

            return Ok("Дисциплина удалена");
        }

        [HttpGet("GetDisciplinesWithGrades")]
        public async Task<IActionResult> GetDisciplinesWithGrades([FromQuery] DisciplineFilter filter,CancellationToken cancellationToken = default)
        {
            var result = await _disciplineService.GetDisciplinesWithGradesAsync(filter, cancellationToken);

            return Ok(result);
        }
    }
}