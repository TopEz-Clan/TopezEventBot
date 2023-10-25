using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopezEventBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduledEventParticipationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountLinkSchedulableEvent");

            migrationBuilder.DropTable(
                name: "EventParticipation");

            migrationBuilder.DropColumn(
                name: "NotificationSent",
                table: "SchedulableEvents");

            migrationBuilder.CreateTable(
                name: "SchedulableEventParticipation",
                columns: table => new
                {
                    EventId = table.Column<long>(type: "INTEGER", nullable: false),
                    AccountLinkId = table.Column<long>(type: "INTEGER", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notified = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulableEventParticipation", x => new { x.AccountLinkId, x.EventId });
                    table.ForeignKey(
                        name: "FK_SchedulableEventParticipation_AccountLinks_AccountLinkId",
                        column: x => x.AccountLinkId,
                        principalTable: "AccountLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchedulableEventParticipation_SchedulableEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "SchedulableEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackableEventParticipation",
                columns: table => new
                {
                    EventId = table.Column<long>(type: "INTEGER", nullable: false),
                    AccountLinkId = table.Column<long>(type: "INTEGER", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartingPoint = table.Column<int>(type: "INTEGER", nullable: false),
                    EndPoint = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackableEventParticipation", x => new { x.AccountLinkId, x.EventId });
                    table.ForeignKey(
                        name: "FK_TrackableEventParticipation_AccountLinks_AccountLinkId",
                        column: x => x.AccountLinkId,
                        principalTable: "AccountLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackableEventParticipation_TrackableEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "TrackableEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchedulableEventParticipation_EventId",
                table: "SchedulableEventParticipation",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackableEventParticipation_EventId",
                table: "TrackableEventParticipation",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchedulableEventParticipation");

            migrationBuilder.DropTable(
                name: "TrackableEventParticipation");

            migrationBuilder.AddColumn<bool>(
                name: "NotificationSent",
                table: "SchedulableEvents",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.CreateTable(
                name: "EventParticipation",
                columns: table => new
                {
                    AccountLinkId = table.Column<long>(type: "INTEGER", nullable: false),
                    EventId = table.Column<long>(type: "INTEGER", nullable: false),
                    EndPoint = table.Column<int>(type: "INTEGER", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartingPoint = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventParticipation", x => new { x.AccountLinkId, x.EventId });
                    table.ForeignKey(
                        name: "FK_EventParticipation_AccountLinks_AccountLinkId",
                        column: x => x.AccountLinkId,
                        principalTable: "AccountLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventParticipation_TrackableEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "TrackableEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountLinkSchedulableEvent_SchedulableEventsId",
                table: "AccountLinkSchedulableEvent",
                column: "SchedulableEventsId");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipation_EventId",
                table: "EventParticipation",
                column: "EventId");
        }
    }
}
