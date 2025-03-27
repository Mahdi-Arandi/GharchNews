using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GharchNews.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitHashtagForUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Hashtags",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hashtags_Id",
                table: "Hashtags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Hashtags_AspNetUsers_Id",
                table: "Hashtags",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hashtags_AspNetUsers_Id",
                table: "Hashtags");

            migrationBuilder.DropIndex(
                name: "IX_Hashtags_Id",
                table: "Hashtags");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Hashtags");
        }
    }
}
