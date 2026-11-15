using JuliaKazakovaKt_41_23.Classes;
using JuliaKazakovaKt_41_23.Filters.GroupFilters;
using JuliaKazakovaKt_41_23.Interfaces.GroupInterfaces;
using JuliaKazakovaKt_41_23.Models;
using Microsoft.AspNetCore.Mvc;

namespace JuliaKazakovaKt_41_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost("by_specialty", Name = "GetGroupsBySpecialty")]
        public async Task<IActionResult> GetGroupsBySpecialtyAsync(
            [FromBody] GroupSpecialtyFilter filter,CancellationToken cancellationToken = default)
        {
            var groups = await _groupService.GetGroupsBySpecialtyAsync(
                filter, cancellationToken);

            return Ok(groups);
        }

        [HttpPost("by_course", Name = "GetGroupsByCourse")]
        public async Task<IActionResult> GetGroupsByCourseAsync([FromBody] GroupCourseFilter filter,CancellationToken cancellationToken = default)
        {
            var groups = await _groupService.GetGroupsByCourseAsync(
                filter, cancellationToken);

            return Ok(groups);
        }

        /*[HttpPost("CreateGroup")]
        public async Task<IActionResult> CreateGroupAsync([FromBody] Group group,CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var groupNew = await _groupService.CreateGroupAsync(group, cancellationToken);
            return Ok(groupNew);
        }*/
        [HttpPost("CreateGroup")]
        public async Task<IActionResult> CreateGroup(GroupCreate dto,CancellationToken cancellationToken)
        {
            var group = new Group
            {
                Name = dto.Name,
                Course = dto.Course,
                SpecialtyId = dto.SpecialtyId
            };

            var result = await _groupService.CreateGroupAsync(
                group, cancellationToken);

            if (result == null)
                return Conflict("Такая группа уже существует");

            return Ok(result);
        }

        [HttpPut("UpdateGroup")]
        public async Task<IActionResult> UpdateGroupAsync([FromBody] Group groupNew,CancellationToken cancellationToken = default)
        {
            var group = await _groupService.UpdateGroupAsync(groupNew, cancellationToken);

            if (group == null)
            {
                return NotFound("Группа не найдена");
            }
            return Ok(group);
        }
        [HttpDelete("DeleteGroup")]
        public async Task<IActionResult> DeleteGroupAsync(int idDelete,CancellationToken cancellationToken = default)
        {
            var groupIsDel = await _groupService.DeleteGroupAsync(
                idDelete, cancellationToken);

            if (!groupIsDel)
            {
                return NotFound("Группа не найдена");
            }
            return Ok("Группа и её студенты удалены");
        }
    }
}