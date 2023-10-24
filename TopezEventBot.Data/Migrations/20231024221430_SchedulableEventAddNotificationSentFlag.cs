using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopezEventBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class SchedulableEventAddNotificationSentFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotificationSent",
                table: "SchedulableEvents",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationSent",
                table: "SchedulableEvents");
        }
    }
}
