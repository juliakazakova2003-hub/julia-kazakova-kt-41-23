using JuliaKazakovaKt_41_23.Classes;
using JuliaKazakovaKt_41_23.Filters.StudentFilters;
using JuliaKazakovaKt_41_23.Interfaces.StudentsInterfaces;
using JuliaKazakovaKt_41_23.Models;
using Microsoft.AspNetCore.Mvc;

namespace JuliaKazakovaKt_41_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly IStudentService _studentService;

        public StudentsController(ILogger<StudentsController> logger, IStudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }

        [HttpPost("by_groupname", Name = "GetStudentsByGroup")]
        public async Task<IActionResult> GetStudentsByGroupAsync([FromBody] StudentGroupFilter filter, CancellationToken cancellationToken = default)
        {
            var students = await _studentService.GetStudentsByGroupAsync(filter, cancellationToken);

            return Ok(students);
        }
        [HttpPost("by_lastname", Name = "GetStudentsByLastName")]
        public async Task<IActionResult> GetStudentsByLastName([FromBody] StudentLastNameFilter filter, CancellationToken cancellationToken = default)
        {
            var students = await _studentService.GetStudentsByLastNameAsync(filter, cancellationToken);

            return Ok(students);
        }
        /*[HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudentAsync([FromBody] Student student,CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentNew = await _studentService.CreateStudentAsync(student, cancellationToken);
            return Ok(studentNew);
        }*/

        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudent(StudentCreate dto,CancellationToken cancellationToken)
        {
            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                GroupId = dto.GroupId
            };

            var result = await _studentService.CreateStudentAsync(student, cancellationToken);

            if (result == null)
            {
                return Conflict("Такой студент уже существует в этой группе");
            }

            return Ok(result);
        }

        [HttpPut("UpdateStudent")]
        public async Task<IActionResult> UpdateStudentAsync([FromBody] Student studentNew,CancellationToken cancellationToken = default)
        {
            var student = await _studentService.UpdateStudentAsync(studentNew, cancellationToken);

            if (student == null)
            {
                return NotFound("Студент не найден");
            }
            return Ok(student);
        }
        [HttpDelete("DeleteStudent")]
        public async Task<IActionResult> DeleteStudentAsync(int idDelete,CancellationToken cancellationToken = default)
        {
            var studentIsDel = await _studentService.DeleteStudentAsync(idDelete, cancellationToken);

            if (!studentIsDel)
            {
                return NotFound("Студент не найден");
            }

            return Ok("Студент удалён");
        }
    }
}
