using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FertilityPoint.DAL.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RescheduledBy",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RescheduledBy",
                table: "Appointments");
        }
    }
}
