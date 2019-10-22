using Microsoft.EntityFrameworkCore.Migrations;

namespace TTTT.Migrations
{
    public partial class user_property_renamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pasword",
                table: "Users",
                newName: "Password");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "Pasword");
        }
    }
}
