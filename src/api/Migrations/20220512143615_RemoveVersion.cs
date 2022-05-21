using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Transleet.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) 
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Components");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Components",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
