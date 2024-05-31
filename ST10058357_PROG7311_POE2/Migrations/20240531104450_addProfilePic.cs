using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10058357_PROG7311_POE2.Migrations
{
    /// <inheritdoc />
    public partial class addProfilePic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductSubCategory_ProductSubCategorySubCategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductSubCategorySubCategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductSubCategorySubCategoryId",
                table: "Product");*/

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "ProductSubCategorySubCategoryId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductSubCategorySubCategoryId",
                table: "Product",
                column: "ProductSubCategorySubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductSubCategory_ProductSubCategorySubCategoryId",
                table: "Product",
                column: "ProductSubCategorySubCategoryId",
                principalTable: "ProductSubCategory",
                principalColumn: "SubCategoryId");
        }
    }
}
