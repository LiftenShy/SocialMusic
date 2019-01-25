using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountManager.Migrations
{
    public partial class RemoveCountFromApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }
    }
}
