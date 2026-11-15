using JuliaKazakovaKt_41_23.Data;
using JuliaKazakovaKt_41_23.Filters.GroupFilters;
using JuliaKazakovaKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace JuliaKazakovaKt_41_23.Interfaces.GroupInterfaces
{
    public interface IGroupService
    {
        Task<Group[]> GetGroupsBySpecialtyAsync(GroupSpecialtyFilter filter,CancellationToken cancellationToken);
        Task<Group[]> GetGroupsByCourseAsync(GroupCourseFilter filter,CancellationToken cancellationToken);
        Task<Group> CreateGroupAsync(Group group,CancellationToken cancellationToken);
        Task<Group> UpdateGroupAsync(Group groupNew, CancellationToken cancellationToken);
        Task<bool> DeleteGroupAsync(int idDelete,CancellationToken cancellationToken);
    }

    public class GroupService : IGroupService
    {
        private readonly StudentDbContext _dbContext;

        public GroupService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Group[]> GetGroupsBySpecialtyAsync(GroupSpecialtyFilter filter,CancellationToken cancellationToken)
        {
            var groups = await _dbContext.Set<Group>()
                .Where(g => g.SpecialtyId == filter.SpecialtyId && !g.IsDeleted)
                .Select(g => new Group
                {
                   GroupId = g.GroupId,
                   Name = g.Name,
                   Course = g.Course,
                   SpecialtyId = g.SpecialtyId
                })
                .ToArrayAsync(cancellationToken);

                return groups;
        }

        public async Task<Group[]> GetGroupsByCourseAsync(GroupCourseFilter filter,CancellationToken cancellationToken)
        {
            var groups = await _dbContext.Set<Group>()
                .Where(g => g.Course == filter.Course && !g.IsDeleted)
                .Select(g => new Group
                {
                    GroupId = g.GroupId,
                    Name = g.Name,
                    Course = g.Course,
                    SpecialtyId = g.SpecialtyId
                })
                .ToArrayAsync(cancellationToken);

                 return groups;
        }

        /*public async Task<Group> CreateGroupAsync(Group group,CancellationToken cancellationToken)
        {
            var groupNew = new Group
            {
                Name = group.Name,
                Course = group.Course,
                SpecialtyId = group.SpecialtyId,
                IsDeleted = false
            };
            _dbContext.Set<Group>().Add(groupNew);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return groupNew;
        }*/
        public async Task<Group> CreateGroupAsync(Group group,CancellationToken cancellationToken)
        {
            var existingGroup = await _dbContext.Set<Group>()
                .FirstOrDefaultAsync(g => g.Name == group.Name && g.SpecialtyId == group.SpecialtyId,cancellationToken);

            if (existingGroup != null)
            {
                return null;
            }

            var groupNew = new Group
            {
                Name = group.Name,
                Course = group.Course,
                SpecialtyId = group.SpecialtyId
            };

            _dbContext.Set<Group>().Add(groupNew);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return groupNew;
        }
        public async Task<Group> UpdateGroupAsync(Group groupNew,CancellationToken cancellationToken)
        {
            var group = await _dbContext.Set<Group>().FirstOrDefaultAsync(g => g.GroupId == groupNew.GroupId);
            if (group == null)
            {
                return null;
            }

            if (groupNew.Name != null && groupNew.Name != "string")
                group.Name = groupNew.Name;
            if (groupNew.Course != 0)
                group.Course = groupNew.Course;
            if (groupNew.SpecialtyId != 0)
                group.SpecialtyId = groupNew.SpecialtyId;

            group.IsDeleted = groupNew.IsDeleted;

            /*if (!group.IsDeleted)
            {
                var students = await _dbContext.Set<Student>()
                    .Where(s => s.GroupId == group.GroupId)
                    .ToListAsync(cancellationToken);

                foreach (var student in students)
                {
                    student.IsDeleted = false;
                }
            }*/
            await _dbContext.SaveChangesAsync(cancellationToken);
            return group;
        }
        public async Task<bool> DeleteGroupAsync(int idDelete,CancellationToken cancellationToken)
        {
            var group = await _dbContext.Set<Group>()
                .FirstOrDefaultAsync(
                    g => g.GroupId == idDelete && !g.IsDeleted,
                    cancellationToken);

            if (group == null)
            {
                return false;
            }

            group.IsDeleted = true;
            var students = await _dbContext.Set<Student>()
                .Where(s => s.GroupId == idDelete && !s.IsDeleted)
                .ToListAsync(cancellationToken);

            foreach (var student in students)
            {
                student.IsDeleted = true;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
