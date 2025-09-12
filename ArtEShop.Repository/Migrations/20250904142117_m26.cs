using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtEShop.Repository.Migrations
{
    /// <inheritdoc />
    public partial class m26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "ArtPieces",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "ArtPieces");
        }
    }
}
