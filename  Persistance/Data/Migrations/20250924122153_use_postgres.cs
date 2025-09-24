using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class use_postgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    PreferredLanguage = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    LastNotifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user_filters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    MinPrice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    MaxPrice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    MinAreaMeterSqr = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    MaxAreaMeterSqr = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    MinRooms = table.Column<int>(type: "integer", nullable: true),
                    MaxRooms = table.Column<int>(type: "integer", nullable: true),
                    MinFloor = table.Column<int>(type: "integer", nullable: true),
                    MaxFloor = table.Column<int>(type: "integer", nullable: true),
                    IsFurnished = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    PetsAllowed = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    HasBalcony = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    NewerThanDays = table.Column<int>(type: "integer", nullable: true),
                    HasAppliances = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_filters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_filters_user_profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "user_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_filters_ProfileId",
                table: "user_filters",
                column: "ProfileId",
                unique: true);

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
            migrationBuilder.DropTable(
                name: "user_filters");

            migrationBuilder.DropTable(
                name: "user_profiles");
        }
    }
}
