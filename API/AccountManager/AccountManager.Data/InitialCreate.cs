using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountManager.Data
{
    public class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("Roles", "Name", "User", "AccountManager");
            migrationBuilder.InsertData("Roles", "Name", "Admin", "AccountManager");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
