using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ex3.Migrations
{
    /// <inheritdoc />
    public partial class RolesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Contact",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_Owner",
                table: "Contact",
                column: "Owner");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_AspNetUsers_Owner",
                table: "Contact",
                column: "Owner",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_AspNetUsers_Owner",
                table: "Contact");

            migrationBuilder.DropIndex(
                name: "IX_Contact_Owner",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "AspNetUsers");
        }
    }
}
