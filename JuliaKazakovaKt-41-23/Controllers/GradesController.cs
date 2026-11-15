using JuliaKazakovaKt_41_23.Filters.GradeFilters;
using JuliaKazakovaKt_41_23.Interfaces.GradeInterfaces;
using JuliaKazakovaKt_41_23.Models;
using Microsoft.AspNetCore.Mvc;

namespace JuliaKazakovaKt_41_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        // Средний балл по дисциплине в группе
        [HttpPost("average_by_group_discipline")]
        public async Task<IActionResult> GetAverageGradeByGroupAndDisciplineAsync([FromBody] GradeGroupDisciplineFilter filter, CancellationToken cancellationToken = default)
        {
            var average =
                await _gradeService.GetAverageGradeByGroupAndDisciplineAsync(
                    filter, cancellationToken);

            return Ok(new { AverageGrade = average });
        }

        // Оценки конкретного студента
        [HttpPost("by_student")]
        public async Task<IActionResult> GetGradesByStudentAsync([FromBody] GradeStudentFilter filter, CancellationToken cancellationToken = default)
        {
            var grades = await _gradeService.GetGradesByStudentAsync(
                filter, cancellationToken);

            return Ok(grades);
        }

        // Средний балл по курсу обучения
        [HttpPost("average_by_course")]
        public async Task<IActionResult> GetAverageGradeByCourseAsync([FromBody] GradeCourseFilter filter, CancellationToken cancellationToken = default)
        {
            var average = await _gradeService.GetAverageGradeByCourseAsync(
                filter, cancellationToken);

            return Ok(new { AverageGrade = average });
        }
        [HttpPost("CreateGrade")]
        public async Task<IActionResult> CreateGradeAsync([FromBody] Grade grade, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var gradeNew = await _gradeService.CreateGradeAsync(grade, cancellationToken);

            if (gradeNew == null)
            {
                return Conflict("Нельзя поставить оценку удалённому студенту или по удалённой дисциплине");
            }

            return Ok(gradeNew);
        }
        [HttpPut("UpdateGrade")]
        public async Task<IActionResult> UpdateGradeAsync([FromBody] Grade gradeNew, CancellationToken cancellationToken = default)
        {
            var grade = await _gradeService.UpdateGradeAsync(gradeNew, cancellationToken);

            if (grade == null)
            {
                return NotFound("Оценка не найдена");
            }
            return Ok(grade);
        }
    }
}
