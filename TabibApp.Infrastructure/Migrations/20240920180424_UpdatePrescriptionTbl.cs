using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TabibApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrescriptionTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "file",
                table: "Prescriptions",
                newName: "fileUrl");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "fileUrl",
                table: "Prescriptions",
                newName: "file");
        }
    }
}
