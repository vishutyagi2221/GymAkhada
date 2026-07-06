using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymAkhada.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendanceApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Attendances",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Attendances");
        }
    }
}
