using Microsoft.EntityFrameworkCore.Migrations;

namespace DynamicFAQ.Data.Migrations
{
    public partial class addedkey1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswer_Section_SectionId",
                table: "QuestionAnswer");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "QuestionAnswer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswer_Section_SectionId",
                table: "QuestionAnswer",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswer_Section_SectionId",
                table: "QuestionAnswer");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "QuestionAnswer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswer_Section_SectionId",
                table: "QuestionAnswer",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
