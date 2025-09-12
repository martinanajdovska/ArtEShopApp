using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtEShop.Repository.Migrations
{
    /// <inheritdoc />
    public partial class m22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Answered",
                table: "Requests",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answered",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Requests");
        }
    }
}
