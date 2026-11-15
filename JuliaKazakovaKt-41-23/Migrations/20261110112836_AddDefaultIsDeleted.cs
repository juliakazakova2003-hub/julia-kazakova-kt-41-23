using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JuliaKazakovaKt_41_23.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultIsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "c_is_deleted",
                table: "cd_student",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Признак удаления",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Признак удаления");

            migrationBuilder.AlterColumn<bool>(
                name: "c_is_deleted",
                table: "cd_group",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Признак удаления",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Признак удаления");

            migrationBuilder.AlterColumn<bool>(
                name: "c_is_deleted",
                table: "cd_discipline",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Признак удаления",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Признак удаления");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "c_is_deleted",
                table: "cd_student",
                type: "bit",
                nullable: false,
                comment: "Признак удаления",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Признак удаления");

            migrationBuilder.AlterColumn<bool>(
                name: "c_is_deleted",
                table: "cd_group",
                type: "bit",
                nullable: false,
                comment: "Признак удаления",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Признак удаления");

            migrationBuilder.AlterColumn<bool>(
                name: "c_is_deleted",
                table: "cd_discipline",
                type: "bit",
                nullable: false,
                comment: "Признак удаления",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Признак удаления");
        }
    }
}
