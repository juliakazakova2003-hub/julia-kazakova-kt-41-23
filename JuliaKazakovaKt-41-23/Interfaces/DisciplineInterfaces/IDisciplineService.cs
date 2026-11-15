using JuliaKazakovaKt_41_23.Data;
using JuliaKazakovaKt_41_23.Filters.DisciplineFilters;
using JuliaKazakovaKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace JuliaKazakovaKt_41_23.Interfaces.DisciplineInterfaces
{
    public interface IDisciplineService
    {
        Task<Discipline[]> GetDisciplinesByDirectionAsync(DisciplineDirectionFilter filter,CancellationToken cancellationToken);
        Task<Discipline> CreateDisciplineAsync(Discipline discipline,CancellationToken cancellationToken);
        Task<Discipline> UpdateDisciplineAsync(Discipline disciplineNew, CancellationToken cancellationToken);
        Task<bool> DeleteDisciplineAsync(int idDelete, CancellationToken cancellationToken);
        //По названию спец вывести дисциплины по которым есть оценки
        Task<List<Discipline>> GetDisciplinesWithGradesAsync(DisciplineFilter filter,CancellationToken cancellationToken);
    }
    public class DisciplineService : IDisciplineService
    {
        private readonly StudentDbContext _dbContext;

        public DisciplineService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Discipline[]> GetDisciplinesByDirectionAsync(DisciplineDirectionFilter filter, CancellationToken cancellationToken)
        {
            var disciplines = await _dbContext.Set<Discipline>()
                .Where(d => d.Direction == filter.Direction && !d.IsDeleted)
                .Select(s => new Discipline
                {
                    DisciplineId = s.DisciplineId,
                    Name = s.Name
                })
                .ToArrayAsync(cancellationToken);
            return disciplines;
        }

        /*public async Task<Discipline> CreateDisciplineAsync(Discipline discipline,CancellationToken cancellationToken)
        {
            var disciplineNew = new Discipline
            {
                Name = discipline.Name,
                Direction = discipline.Direction,
                IsDeleted = false,
            };

            _dbContext.Set<Discipline>().Add(disciplineNew);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return disciplineNew;
        }*/

        public async Task<Discipline> CreateDisciplineAsync(Discipline discipline, CancellationToken cancellationToken)
        {
            var existingDiscipline = await _dbContext.Set<Discipline>()
                .FirstOrDefaultAsync(d =>d.Name == discipline.Name && d.Direction == discipline.Direction,cancellationToken);

            if (existingDiscipline != null)
            {
                return null;
            }

            var disciplineNew = new Discipline
            {
                Name = discipline.Name,
                Direction = discipline.Direction
            };

            _dbContext.Set<Discipline>().Add(disciplineNew);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return disciplineNew;
        }

        public async Task<Discipline> UpdateDisciplineAsync(Discipline disciplineNew,CancellationToken cancellationToken)
        {
            var discipline = await _dbContext.Set<Discipline>().FirstOrDefaultAsync(d => d.DisciplineId == disciplineNew.DisciplineId);

            if (discipline == null)
            {
                return null;
            }

            if (disciplineNew.Name != null && disciplineNew.Name != "string")
                discipline.Name = disciplineNew.Name;
            if (disciplineNew.Direction != null && disciplineNew.Direction != "string")
                discipline.Direction = disciplineNew.Direction;
            discipline.IsDeleted = disciplineNew.IsDeleted;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return discipline;
        }

        public async Task<bool> DeleteDisciplineAsync(int idDelete,CancellationToken cancellationToken)
        {
            var discipline = await _dbContext.Set<Discipline>()
                .FirstOrDefaultAsync(d => d.DisciplineId == idDelete && !d.IsDeleted);

            if (discipline == null)
            {
                return false;
            }

            discipline.IsDeleted = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<List<Discipline>> GetDisciplinesWithGradesAsync(DisciplineFilter filter,CancellationToken cancellationToken)
        {
            var disciplines = await _dbContext.Set<Discipline>().Where(d => !d.IsDeleted && d.Grades.Any(g =>
                        !g.Student.IsDeleted &&
                        !g.Student.Group.IsDeleted &&
                        g.Student.Group.Specialty.Title == filter.SpecialtyTitle
                    ))
                .Select(d => new Discipline
                {
                    DisciplineId = d.DisciplineId,
                    Name = d.Name
                })
                .ToListAsync(cancellationToken);

            return disciplines;
        }
    }
}
