using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameShop.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaylistAndGameRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayListId",
                table: "Playlists",
                newName: "PlaylistID");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Playlists",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Playlists");

            migrationBuilder.RenameColumn(
                name: "PlaylistID",
                table: "Playlists",
                newName: "PlayListId");
        }
    }
}
