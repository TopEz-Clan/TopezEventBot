using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopezEventBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameSchedulableProgressToTrackable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchedulableEventProgress");

            migrationBuilder.CreateTable(
                name: "TrackableEventProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Progress = table.Column<int>(type: "INTEGER", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TrackableEventParticipationAccountLinkId = table.Column<long>(type: "INTEGER", nullable: true),
                    TrackableEventParticipationEventId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackableEventProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackableEventProgress_TrackableEventParticipation_TrackableEventParticipationAccountLinkId_TrackableEventParticipationEventId",
                        columns: x => new { x.TrackableEventParticipationAccountLinkId, x.TrackableEventParticipationEventId },
                        principalTable: "TrackableEventParticipation",
                        principalColumns: new[] { "AccountLinkId", "EventId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackableEventProgress_TrackableEventParticipationAccountLinkId_TrackableEventParticipationEventId",
                table: "TrackableEventProgress",
                columns: new[] { "TrackableEventParticipationAccountLinkId", "TrackableEventParticipationEventId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackableEventProgress");

            migrationBuilder.CreateTable(
                name: "SchedulableEventProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Progress = table.Column<int>(type: "INTEGER", nullable: false),
                    TrackableEventParticipationAccountLinkId = table.Column<long>(type: "INTEGER", nullable: true),
                    TrackableEventParticipationEventId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulableEventProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedulableEventProgress_TrackableEventParticipation_TrackableEventParticipationAccountLinkId_TrackableEventParticipationEventId",
                        columns: x => new { x.TrackableEventParticipationAccountLinkId, x.TrackableEventParticipationEventId },
                        principalTable: "TrackableEventParticipation",
                        principalColumns: new[] { "AccountLinkId", "EventId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchedulableEventProgress_TrackableEventParticipationAccountLinkId_TrackableEventParticipationEventId",
                table: "SchedulableEventProgress",
                columns: new[] { "TrackableEventParticipationAccountLinkId", "TrackableEventParticipationEventId" });
        }
    }
}
