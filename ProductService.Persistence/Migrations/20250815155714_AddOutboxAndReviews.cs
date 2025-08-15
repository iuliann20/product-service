using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxAndReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ProductReviews",
                newName: "CreatedAtUtc");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "ProductReviews",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                table: "ProductReviews",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProductReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OccurredOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductId1",
                table: "ProductReviews",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductId1",
                table: "ProductReviews",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductId1",
                table: "ProductReviews");

            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.DropIndex(
                name: "IX_ProductReviews_ProductId1",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProductReviews");

            migrationBuilder.RenameColumn(
                name: "CreatedAtUtc",
                table: "ProductReviews",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "ProductReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);
        }
    }
}
