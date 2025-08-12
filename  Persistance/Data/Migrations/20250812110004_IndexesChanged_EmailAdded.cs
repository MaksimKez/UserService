using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class IndexesChanged_EmailAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_profiles_LastNotifiedAt",
                table: "user_profiles");

            migrationBuilder.DropIndex(
                name: "IX_user_profiles_PreferredLanguage",
                table: "user_profiles");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "user_profiles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_user_profiles_Email",
                table: "user_profiles",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_user_profiles_FirstName",
                table: "user_profiles",
                column: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_profiles_Email",
                table: "user_profiles");

            migrationBuilder.DropIndex(
                name: "IX_user_profiles_FirstName",
                table: "user_profiles");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "user_profiles");

            migrationBuilder.CreateIndex(
                name: "IX_user_profiles_LastNotifiedAt",
                table: "user_profiles",
                column: "LastNotifiedAt");

            migrationBuilder.CreateIndex(
                name: "IX_user_profiles_PreferredLanguage",
                table: "user_profiles",
                column: "PreferredLanguage");
        }
    }
}
