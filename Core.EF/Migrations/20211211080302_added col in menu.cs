using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.EF.Migrations
{
    public partial class addedcolinmenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_old_link",
                table: "menu",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_old_link",
                table: "menu");
        }
    }
}
