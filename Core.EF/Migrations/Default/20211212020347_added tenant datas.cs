using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.EF.Migrations.Default
{
    public partial class addedtenantdatas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "identifier",
                table: "tenant",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.InsertData(
                table: "tenant",
                columns: new[] { "id", "database", "database_type", "identifier", "password", "port", "server", "user" },
                values: new object[,]
                {
                    { new Guid("4249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1"), "multitenant_dev_db", 1, "sme1:5001", "Admin", "5432", "localhost", "postgres" },
                    { new Guid("5249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1"), "multitenant1_dev_db", 1, "sme2:5001", "Admin", "5432", "localhost", "postgres" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tenant",
                keyColumn: "id",
                keyValue: new Guid("4249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1"));

            migrationBuilder.DeleteData(
                table: "tenant",
                keyColumn: "id",
                keyValue: new Guid("5249f843-d4a3-4d9c-b0ff-bc1a9d3cd5e1"));

            migrationBuilder.DropColumn(
                name: "identifier",
                table: "tenant");
        }
    }
}
