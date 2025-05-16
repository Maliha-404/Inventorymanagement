using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventorymanagement.Migrations
{
    /// <inheritdoc />
    public partial class AddActionToStockMovement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "StockMovements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "StockMovements");
        }
    }
}
