using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meetups.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizerReviewtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizerReviews",
                columns: table => new
                {
                    OrganizerReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewerUserId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrganizerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizerReviews", x => x.OrganizerReviewId);
                    table.ForeignKey(
                        name: "FK_OrganizerReviews_Users_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizerReviews_Users_ReviewerUserId",
                        column: x => x.ReviewerUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizerReviews_OrganizerId",
                table: "OrganizerReviews",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizerReviews_ReviewerUserId",
                table: "OrganizerReviews",
                column: "ReviewerUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizerReviews");
        }
    }
}
