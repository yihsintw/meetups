using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meetups.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class addtablepayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "RSVPs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "RSVPs");
        }
    }
}
