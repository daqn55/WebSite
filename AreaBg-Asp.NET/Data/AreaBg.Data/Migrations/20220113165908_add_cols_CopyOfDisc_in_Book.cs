using Microsoft.EntityFrameworkCore.Migrations;

namespace AreaBg.Data.Migrations
{
    public partial class add_cols_CopyOfDisc_in_Book : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CopyOfDiscount",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CopyOfDiscount",
                table: "Books");
        }
    }
}
