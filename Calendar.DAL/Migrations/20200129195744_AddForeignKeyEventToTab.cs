using Microsoft.EntityFrameworkCore.Migrations;

namespace Calendar.DAL.Migrations
{
    public partial class AddForeignKeyEventToTab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Tabs_TabId",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "TabId",
                table: "Events",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Tabs_TabId",
                table: "Events",
                column: "TabId",
                principalTable: "Tabs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Tabs_TabId",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "TabId",
                table: "Events",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Tabs_TabId",
                table: "Events",
                column: "TabId",
                principalTable: "Tabs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
