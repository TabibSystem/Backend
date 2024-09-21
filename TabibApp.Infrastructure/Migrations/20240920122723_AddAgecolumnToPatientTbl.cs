using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TabibApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAgecolumnToPatientTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "MedicalHistoryRecords");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "MedicalHistoryRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "MedicalHistoryRecords");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "MedicalHistoryRecords",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
