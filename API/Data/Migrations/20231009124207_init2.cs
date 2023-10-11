using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Images",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Comments",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Albums",
                newName: "CreationDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Images",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Comments",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Albums",
                newName: "Date");
        }
    }
}
