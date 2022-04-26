using Microsoft.EntityFrameworkCore.Migrations;

namespace AreaBg.Data.Migrations
{
    public partial class addtobooksubcatId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Subcategories_SubcategoryId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "SubcategoryId",
                table: "Books",
                newName: "SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_SubcategoryId",
                table: "Books",
                newName: "IX_Books_SubCategoryId");

            migrationBuilder.AlterColumn<int>(
                name: "SubCategoryId",
                table: "Books",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Subcategories_SubCategoryId",
                table: "Books",
                column: "SubCategoryId",
                principalTable: "Subcategories",
                principalColumn: "SubcategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Subcategories_SubCategoryId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "SubCategoryId",
                table: "Books",
                newName: "SubcategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_SubCategoryId",
                table: "Books",
                newName: "IX_Books_SubcategoryId");

            migrationBuilder.AlterColumn<int>(
                name: "SubcategoryId",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Subcategories_SubcategoryId",
                table: "Books",
                column: "SubcategoryId",
                principalTable: "Subcategories",
                principalColumn: "SubcategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
