using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymAkhada.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GymCategories",
                columns: table => new
                {
                    Gym_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gym_categoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gym_Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymCategories", x => x.Gym_ID);
                });

            migrationBuilder.CreateTable(
                name: "GymMembers",
                columns: table => new
                {
                    GymMember_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MemberType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AadharNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    WeightKg = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gym_ID = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymMembers", x => x.GymMember_ID);
                    table.ForeignKey(
                        name: "FK_GymMembers_GymCategories_Gym_ID",
                        column: x => x.Gym_ID,
                        principalTable: "GymCategories",
                        principalColumn: "Gym_ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "GymSubcategories",
                columns: table => new
                {
                    GymSub_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gym_ID = table.Column<int>(type: "int", nullable: false),
                    GymSub_age = table.Column<int>(type: "int", nullable: false),
                    GymWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymSubcategories", x => x.GymSub_ID);
                    table.ForeignKey(
                        name: "FK_GymSubcategories_GymCategories_Gym_ID",
                        column: x => x.Gym_ID,
                        principalTable: "GymCategories",
                        principalColumn: "Gym_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GymMembers_Gym_ID",
                table: "GymMembers",
                column: "Gym_ID");

            migrationBuilder.CreateIndex(
                name: "IX_GymSubcategories_Gym_ID",
                table: "GymSubcategories",
                column: "Gym_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GymMembers");

            migrationBuilder.DropTable(
                name: "GymSubcategories");

            migrationBuilder.DropTable(
                name: "GymCategories");
        }
    }
}
