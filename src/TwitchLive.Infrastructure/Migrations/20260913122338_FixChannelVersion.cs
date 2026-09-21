using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitchLive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixChannelVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "channels",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldRowVersion: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Version",
                table: "channels",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 1);
        }
    }
}
