using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymAkhada.Migrations
{
    /// <inheritdoc />
    public partial class AddTournamentRegistrationSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "GymWeight",
                table: "GymSubcategories",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "GymSub_age",
                table: "GymSubcategories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "EntryFee",
                table: "GymSubcategories",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImportantDetails",
                table: "GymSubcategories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubcategoryName",
                table: "GymSubcategories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TournamentRegistrations",
                columns: table => new
                {
                    RegistrationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    GymMember_ID = table.Column<int>(type: "int", nullable: false),
                    GymSub_ID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentRegistrations", x => x.RegistrationId);
                    table.ForeignKey(
                        name: "FK_TournamentRegistrations_GymMembers_GymMember_ID",
                        column: x => x.GymMember_ID,
                        principalTable: "GymMembers",
                        principalColumn: "GymMember_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentRegistrations_GymSubcategories_GymSub_ID",
                        column: x => x.GymSub_ID,
                        principalTable: "GymSubcategories",
                        principalColumn: "GymSub_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentRegistrations_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRegistrations_GymMember_ID",
                table: "TournamentRegistrations",
                column: "GymMember_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRegistrations_GymSub_ID",
                table: "TournamentRegistrations",
                column: "GymSub_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRegistrations_TournamentId",
                table: "TournamentRegistrations",
                column: "TournamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TournamentRegistrations");

            migrationBuilder.DropColumn(
                name: "EntryFee",
                table: "GymSubcategories");

            migrationBuilder.DropColumn(
                name: "ImportantDetails",
                table: "GymSubcategories");

            migrationBuilder.DropColumn(
                name: "SubcategoryName",
                table: "GymSubcategories");

            migrationBuilder.AlterColumn<decimal>(
                name: "GymWeight",
                table: "GymSubcategories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GymSub_age",
                table: "GymSubcategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
