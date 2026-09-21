using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitchLive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addNewIsModerator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsModerated",
                table: "channels",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsModerated",
                table: "channels");
        }
    }
}
