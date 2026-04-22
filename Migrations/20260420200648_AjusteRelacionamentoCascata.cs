using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_iALL.Migrations
{
    /// <inheritdoc />
    public partial class AjusteRelacionamentoCascata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestHistories_Collaborators_CollaboratorId",
                table: "RequestHistories");

            migrationBuilder.AddColumn<int>(
                name: "RequesterId",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequesterId",
                table: "Requests",
                column: "RequesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestHistories_Collaborators_CollaboratorId",
                table: "RequestHistories",
                column: "CollaboratorId",
                principalTable: "Collaborators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Collaborators_RequesterId",
                table: "Requests",
                column: "RequesterId",
                principalTable: "Collaborators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestHistories_Collaborators_CollaboratorId",
                table: "RequestHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Collaborators_RequesterId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequesterId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequesterId",
                table: "Requests");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestHistories_Collaborators_CollaboratorId",
                table: "RequestHistories",
                column: "CollaboratorId",
                principalTable: "Collaborators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
