using JuliaKazakovaKt_41_23.Database.Helpers;
using JuliaKazakovaKt_41_23.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuliaKazakovaKt_41_23.Database.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        private const string TableName = "cd_group";

        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder
                .HasKey(p => p.GroupId)
                .HasName($"pk_{TableName}_group_id");

            builder.Property(p => p.GroupId)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.GroupId)
                .HasColumnName("group_id")
                .HasComment("Идентификатор группы");

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnName("c_group_name")
                .HasColumnType(ColumnType.String)
                .HasMaxLength(100)
                .HasComment("Название группы");

            builder.Property(p => p.Course)
                .IsRequired()
                .HasColumnName("c_course")
                .HasColumnType(ColumnType.Int)
                .HasComment("Курс");

            builder.Property(p => p.SpecialtyId)
                .IsRequired()
                .HasColumnName("f_specialty_id")
                .HasColumnType(ColumnType.Int)
                .HasComment("Идентификатор специальности");

            builder.Property(p => p.IsDeleted)
                .IsRequired()
                .HasColumnName("c_is_deleted")
                .HasColumnType(ColumnType.Bool)
                .HasComment("Признак удаления");

            builder.ToTable(TableName)
                .HasOne(p => p.Specialty)
                .WithMany(t => t.Groups)
                .HasForeignKey(p => p.SpecialtyId)
                .HasConstraintName("fk_f_specialty_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(TableName)
                .HasIndex(p => p.SpecialtyId,
                    $"idx_{TableName}_fk_f_specialty_id");

            builder.Navigation(p => p.Specialty)
                .AutoInclude();
        }
    }
}