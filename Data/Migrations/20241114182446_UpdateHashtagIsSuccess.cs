using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GharchNews.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHashtagIsSuccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "Hashtags",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuccess",
                table: "Hashtags");
        }
    }
}
