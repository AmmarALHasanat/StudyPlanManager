using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Tasks",
                newName: "Target");

            migrationBuilder.RenameColumn(
                name: "Pages",
                table: "Tasks",
                newName: "Industry");

            migrationBuilder.RenameColumn(
                name: "Book",
                table: "Tasks",
                newName: "ActivityName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Target",
                table: "Tasks",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Industry",
                table: "Tasks",
                newName: "Pages");

            migrationBuilder.RenameColumn(
                name: "ActivityName",
                table: "Tasks",
                newName: "Book");
        }
    }
}
