using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Max_movie.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFechaReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Reviews",
                newName: "FechaReview");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaReview",
                table: "Reviews",
                newName: "Fecha");
        }
    }
}
