using Microsoft.AspNetCore.Mvc;
using JuliaKazakovaKt_41_23.Filters.StudentFilters;
using JuliaKazakovaKt_41_23.Interfaces.StudentsInterfaces;

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
        public async Task<IActionResult> GetStudentsByGroupAsync(StudentGroupFilter filter, CancellationToken cancellationToken = default)
        {
            var students = await _studentService.GetStudentsByGroupAsync(filter, cancellationToken);

            return Ok(students);
        }
        [HttpPost("by_lastname", Name = "GetStudentsByLastName")]
        public async Task<IActionResult> GetStudentsByLastName(StudentLastNameFilter filter, CancellationToken cancellationToken = default)
        {
            var students = await _studentService.GetStudentsByLastNameAsync(filter, cancellationToken);

            return Ok(students);
        }

    }
}
