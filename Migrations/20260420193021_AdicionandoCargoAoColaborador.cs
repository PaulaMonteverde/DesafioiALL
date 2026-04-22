using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_iALL.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoCargoAoColaborador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "role",
                table: "Collaborators",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "Collaborators");
        }
    }
}
