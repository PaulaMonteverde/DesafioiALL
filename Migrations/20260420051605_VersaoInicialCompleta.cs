using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_iALL.Migrations
{
    /// <inheritdoc />
    public partial class VersaoInicialCompleta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_requestedItems_Items_ItemId",
                table: "requestedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_requestedItems_requests_RequestModelId",
                table: "requestedItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_requests",
                table: "requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_requestedItems",
                table: "requestedItems");

            migrationBuilder.RenameTable(
                name: "requests",
                newName: "Requests");

            migrationBuilder.RenameTable(
                name: "requestedItems",
                newName: "RequestedItems");

            migrationBuilder.RenameIndex(
                name: "IX_requestedItems_RequestModelId",
                table: "RequestedItems",
                newName: "IX_RequestedItems_RequestModelId");

            migrationBuilder.RenameIndex(
                name: "IX_requestedItems_ItemId",
                table: "RequestedItems",
                newName: "IX_RequestedItems_ItemId");

            migrationBuilder.AddColumn<bool>(
                name: "IsApprovedByDirector",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApprovedByManager",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApprovedBySupplies",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalValue",
                table: "Requests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalValue",
                table: "RequestedItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Items",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requests",
                table: "Requests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestedItems",
                table: "RequestedItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Collaborators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CollaboratorId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestHistories_Collaborators_CollaboratorId",
                        column: x => x.CollaboratorId,
                        principalTable: "Collaborators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestHistories_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestHistories_CollaboratorId",
                table: "RequestHistories",
                column: "CollaboratorId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestHistories_RequestId",
                table: "RequestHistories",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestedItems_Items_ItemId",
                table: "RequestedItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestedItems_Requests_RequestModelId",
                table: "RequestedItems",
                column: "RequestModelId",
                principalTable: "Requests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestedItems_Items_ItemId",
                table: "RequestedItems");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestedItems_Requests_RequestModelId",
                table: "RequestedItems");

            migrationBuilder.DropTable(
                name: "RequestHistories");

            migrationBuilder.DropTable(
                name: "Collaborators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requests",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestedItems",
                table: "RequestedItems");

            migrationBuilder.DropColumn(
                name: "IsApprovedByDirector",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsApprovedByManager",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsApprovedBySupplies",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "TotalValue",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "TotalValue",
                table: "RequestedItems");

            migrationBuilder.RenameTable(
                name: "Requests",
                newName: "requests");

            migrationBuilder.RenameTable(
                name: "RequestedItems",
                newName: "requestedItems");

            migrationBuilder.RenameIndex(
                name: "IX_RequestedItems_RequestModelId",
                table: "requestedItems",
                newName: "IX_requestedItems_RequestModelId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestedItems_ItemId",
                table: "requestedItems",
                newName: "IX_requestedItems_ItemId");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "Items",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_requests",
                table: "requests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_requestedItems",
                table: "requestedItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_requestedItems_Items_ItemId",
                table: "requestedItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_requestedItems_requests_RequestModelId",
                table: "requestedItems",
                column: "RequestModelId",
                principalTable: "requests",
                principalColumn: "Id");
        }
    }
}
