using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtEShop.Repository.Migrations
{
    /// <inheritdoc />
    public partial class m33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "Answered",
                table: "Requests",
                newName: "IsAnswered");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAnswered",
                table: "Requests",
                newName: "Answered");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Requests",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
