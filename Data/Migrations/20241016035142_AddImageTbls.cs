using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GharchNews.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageTbls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Galleries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Galleries");
        }
    }
}
