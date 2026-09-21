using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitchLive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addFollowerDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "followers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FromUserId = table.Column<string>(type: "TEXT", nullable: false),
                    FromLogin = table.Column<string>(type: "TEXT", nullable: false),
                    FromUserName = table.Column<string>(type: "TEXT", nullable: false),
                    ToUserId = table.Column<string>(type: "TEXT", nullable: false),
                    ToLogin = table.Column<string>(type: "TEXT", nullable: false),
                    ToUserName = table.Column<string>(type: "TEXT", nullable: false),
                    FollowedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Recognition = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_followers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "followers");
        }
    }
}
