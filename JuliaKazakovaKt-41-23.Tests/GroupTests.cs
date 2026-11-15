using JuliaKazakovaKt_41_23.Models;
using Regex = System.Text.RegularExpressions.Regex;
using Xunit;

namespace JuliaKazakovaKt_41_23.Tests
{
    public class GroupTests
    {
        [Fact]
        public void IsValidName_ValidGroupName_True()
        {
            // название состоит из букв, дефиса и чисел,а  последние две цифры обозначают номер подгруппы.
            var testGroup = new Group { Name = "КТ-41-23" };

            Assert.True(testGroup.IsValidName());
        }

        [Fact]
        public void IsValidName_InvalidGroupName_False()
        {
            var testGroup = new Group { Name = "гр1" };

            Assert.False(testGroup.IsValidName());
        }

        [Fact]
        public void SpecialtyCode_HasTwoDots()
        {
            var specialty = new Specialty { Code = "09.02.07" };

            // код специальности должен иметь вид NN.NN.NN
            Assert.Matches(new Regex(@"^\d{2}\.\d{2}\.\d{2}$"), specialty.Code);
        }

        [Fact]
        public void SpecialtyTitle_StartsWithCapitalLetter()
        {
            var specialty = new Specialty { Title = "Информационные системы" };

            // название начинается с заглавной русской буквы, затем идут строчные буквы
            Assert.Matches(new Regex(@"^[А-ЯЁ][а-яё]*(?: [а-яё]+)*$"), specialty.Title);
        }

        [Fact]
        public void SpecialtyTitle_UsesSingleSpacesBetweenWords()
        {
            var specialty = new Specialty { Title = "Информационные системы" };

            // между словами допускается один обычный пробел; двойные пробелы запрещены.
            Assert.DoesNotMatch(new Regex(@"\s{2,}"), specialty.Title);
            Assert.DoesNotMatch(new Regex(@"^\s|\s$"), specialty.Title);
        }
    }
}
