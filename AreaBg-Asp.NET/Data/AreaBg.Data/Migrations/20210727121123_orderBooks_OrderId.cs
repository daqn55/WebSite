using Microsoft.EntityFrameworkCore.Migrations;

namespace AreaBg.Data.Migrations
{
    public partial class orderBooks_OrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderBooks_Orders_OrderId",
                table: "OrderBooks");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrderBooks",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderBooks_Orders_OrderId",
                table: "OrderBooks",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderBooks_Orders_OrderId",
                table: "OrderBooks");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrderBooks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_OrderBooks_Orders_OrderId",
                table: "OrderBooks",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
