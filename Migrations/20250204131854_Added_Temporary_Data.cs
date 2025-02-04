using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoseAPI.Migrations
{
    /// <inheritdoc />
    public partial class Added_Temporary_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TemporaryData",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemporaryData",
                table: "User");
        }
    }
}
