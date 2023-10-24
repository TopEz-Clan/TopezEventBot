using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopezEventBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSchedulableEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipation_Events_EventId",
                table: "EventParticipation");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.CreateTable(
                name: "SchedulableEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ScheduledAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Activity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulableEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackableEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Activity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackableEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountLinkSchedulableEvent",
                columns: table => new
                {
                    ParticipantsId = table.Column<long>(type: "INTEGER", nullable: false),
                    SchedulableEventsId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountLinkSchedulableEvent", x => new { x.ParticipantsId, x.SchedulableEventsId });
                    table.ForeignKey(
                        name: "FK_AccountLinkSchedulableEvent_AccountLinks_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "AccountLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountLinkSchedulableEvent_SchedulableEvents_SchedulableEventsId",
                        column: x => x.SchedulableEventsId,
                        principalTable: "SchedulableEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountLinkSchedulableEvent_SchedulableEventsId",
                table: "AccountLinkSchedulableEvent",
                column: "SchedulableEventsId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipation_TrackableEvents_EventId",
                table: "EventParticipation",
                column: "EventId",
                principalTable: "TrackableEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipation_TrackableEvents_EventId",
                table: "EventParticipation");

            migrationBuilder.DropTable(
                name: "AccountLinkSchedulableEvent");

            migrationBuilder.DropTable(
                name: "TrackableEvents");

            migrationBuilder.DropTable(
                name: "SchedulableEvents");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Activity = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipation_Events_EventId",
                table: "EventParticipation",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
