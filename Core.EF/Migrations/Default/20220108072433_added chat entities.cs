using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.EF.Migrations.Default
{
    public partial class addedchatentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "profile_picture_data_url",
                schema: "dbo",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "chat_histories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    from_user_id = table.Column<string>(type: "text", nullable: true),
                    to_user_id = table.Column<string>(type: "text", nullable: true),
                    message = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    from_user_id1 = table.Column<Guid>(type: "uuid", nullable: true),
                    to_user_id1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_histories", x => x.id);
                    table.ForeignKey(
                        name: "fk_chat_histories_user_from_user_id1",
                        column: x => x.from_user_id1,
                        principalSchema: "dbo",
                        principalTable: "user",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_chat_histories_user_to_user_id1",
                        column: x => x.to_user_id1,
                        principalSchema: "dbo",
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_chat_histories_from_user_id1",
                table: "chat_histories",
                column: "from_user_id1");

            migrationBuilder.CreateIndex(
                name: "ix_chat_histories_to_user_id1",
                table: "chat_histories",
                column: "to_user_id1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_histories");

            migrationBuilder.DropColumn(
                name: "profile_picture_data_url",
                schema: "dbo",
                table: "user");
        }
    }
}
