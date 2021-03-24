using Microsoft.EntityFrameworkCore.Migrations;

namespace TGbot.Migrations
{
    public partial class add_wait_message_to_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WaitMessageId",
                table: "User",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaitMessageId",
                table: "User");
        }
    }
}
