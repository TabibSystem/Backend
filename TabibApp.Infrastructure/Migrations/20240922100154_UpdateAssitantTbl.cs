using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TabibApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssitantTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Assistants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Assistants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
