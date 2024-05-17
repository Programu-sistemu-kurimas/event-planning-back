﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Event_planning_back.Migrations
{
    /// <inheritdoc />
    public partial class SetDefaultForIsArchived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Projects");
        }
    }
}
