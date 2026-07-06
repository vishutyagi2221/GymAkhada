using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymAkhada.Migrations
{
    /// <inheritdoc />
    public partial class AddPrizeDistribution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BronzePrize",
                table: "Tournaments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GoldPrize",
                table: "Tournaments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SilverPrize",
                table: "Tournaments",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BronzePrize",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "GoldPrize",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "SilverPrize",
                table: "Tournaments");
        }
    }
}
