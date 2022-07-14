using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FertilityPoint.DAL.Migrations
{
    public partial class _13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "PaypalPayments",
                newName: "PayerLastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "PaypalPayments",
                newName: "PayerFirstName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "PaypalPayments",
                newName: "PayerEmail");

            migrationBuilder.RenameColumn(
                name: "CountryCode",
                table: "PaypalPayments",
                newName: "PayerCountryCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayerLastName",
                table: "PaypalPayments",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "PayerFirstName",
                table: "PaypalPayments",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "PayerEmail",
                table: "PaypalPayments",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "PayerCountryCode",
                table: "PaypalPayments",
                newName: "CountryCode");
        }
    }
}
