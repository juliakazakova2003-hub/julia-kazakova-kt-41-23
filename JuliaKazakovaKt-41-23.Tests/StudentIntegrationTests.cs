
using JuliaKazakovaKt_41_23.Data;
using JuliaKazakovaKt_41_23.Filters.StudentFilters;
using JuliaKazakovaKt_41_23.Interfaces.StudentsInterfaces;
using JuliaKazakovaKt_41_23.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JuliaKazakovaKt_41_23.Tests
{
    public class StudentIntegrationTests
    {
        private readonly DbContextOptions<StudentDbContext> _dbContextOptions;

        public StudentIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private async Task SeedData(StudentDbContext ctx)
        {
            var groups = new List<Group>
            {
                new Group
                {
                    Name = "гр1",
                    Course = 1,
                    SpecialtyId = 1
                },
                new Group
                {
                    Name = "гр2",
                    Course = 1,
                    SpecialtyId = 2
                }
            };

            await ctx.Set<Group>().AddRangeAsync(groups);
            await ctx.SaveChangesAsync();

            var students = new List<Student>
            {
                new Student
                {
                    LastName = "иванов",
                    FirstName = "иван",
                    GroupId = groups[0].GroupId
                },
                new Student
                {
                    LastName = "Иванов",
                    FirstName = "петр",
                    GroupId = groups[0].GroupId
                },
                new Student
                {
                    LastName = "петров",
                    FirstName = "иван",
                    GroupId = groups[1].GroupId
                },
                new Student
                {
                    LastName = "Сидоров",
                    FirstName = "петр",
                    GroupId = groups[0].GroupId
                }
            };

            await ctx.Set<Student>().AddRangeAsync(students);
            await ctx.SaveChangesAsync();
        }

        // получение студентов группы гр1
        [Fact]
        public async Task GetStudentsByGroupAsync_grp1_ThreeObjects()
        {
            using var ctx = new StudentDbContext(_dbContextOptions);

            await SeedData(ctx);

            var studentService = new StudentService(ctx);

            var filter = new StudentGroupFilter
            {
                GroupName = "гр1"
            };

            var result = await studentService.GetStudentsByGroupAsync(
                filter,
                CancellationToken.None);

            Assert.Equal(3, result.Length);
        }

        // получение студентов по фамилии иванов
        [Fact]
        public async Task GetStudentsByLastNameAsync_ivanov_OneObject()
        {
            using var ctx = new StudentDbContext(_dbContextOptions);

            await SeedData(ctx);

            var studentService = new StudentService(ctx);

            var filter = new StudentLastNameFilter
            {
                LastName = "иванов"
            };

            var result = await studentService.GetStudentsByLastNameAsync(
                filter,
                CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("иван", result[0].FirstName);
        }

        [Fact]
        public async Task GetStudentsByLastNameAsync_Ivanov_OneObject()
        {
            using var ctx = new StudentDbContext(_dbContextOptions);

            await SeedData(ctx);

            var studentService = new StudentService(ctx);

            var filter = new StudentLastNameFilter
            {
                LastName = "Иванов"
            };

            var result = await studentService.GetStudentsByLastNameAsync(
                filter,
                CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("петр", result[0].FirstName);
        }
    }
}
