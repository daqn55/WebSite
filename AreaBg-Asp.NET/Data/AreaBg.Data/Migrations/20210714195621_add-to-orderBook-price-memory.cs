using Microsoft.EntityFrameworkCore.Migrations;

namespace AreaBg.Data.Migrations
{
    public partial class addtoorderBookpricememory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderBooks_Books_BookId",
                table: "OrderBooks");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "OrderBooks",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "OrderBooks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderBooks",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceWithDisc",
                table: "OrderBooks",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderBooks_Books_BookId",
                table: "OrderBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderBooks_Books_BookId",
                table: "OrderBooks");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "OrderBooks");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderBooks");

            migrationBuilder.DropColumn(
                name: "PriceWithDisc",
                table: "OrderBooks");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "OrderBooks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_OrderBooks_Books_BookId",
                table: "OrderBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
