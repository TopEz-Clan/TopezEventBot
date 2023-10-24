﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TopezEventBot.Data.Context;

#nullable disable

namespace TopezEventBot.Data.Migrations
{
    [DbContext(typeof(TopezContext))]
    [Migration("20231024221430_SchedulableEventAddNotificationSentFlag")]
    partial class SchedulableEventAddNotificationSentFlag
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("AccountLinkSchedulableEvent", b =>
                {
                    b.Property<long>("ParticipantsId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SchedulableEventsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ParticipantsId", "SchedulableEventsId");

                    b.HasIndex("SchedulableEventsId");

                    b.ToTable("AccountLinkSchedulableEvent");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.AccountLink", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("DiscordMemberId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RunescapeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AccountLinks");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.EventParticipation", b =>
                {
                    b.Property<long>("AccountLinkId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("EventId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EndPoint")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("RegisteredAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("StartingPoint")
                        .HasColumnType("INTEGER");

                    b.HasKey("AccountLinkId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("EventParticipation");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.GuildWarningChannel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("WarningChannelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("GuildWarningChannels");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.SchedulableEvent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Activity")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NotificationSent")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("ScheduledAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SchedulableEvents");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.TrackableEvent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Activity")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("TrackableEvents");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.Warning", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<ulong>("WarnedBy")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("WarnedUser")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Warnings");
                });

            modelBuilder.Entity("AccountLinkSchedulableEvent", b =>
                {
                    b.HasOne("TopezEventBot.Data.Entities.AccountLink", null)
                        .WithMany()
                        .HasForeignKey("ParticipantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TopezEventBot.Data.Entities.SchedulableEvent", null)
                        .WithMany()
                        .HasForeignKey("SchedulableEventsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.EventParticipation", b =>
                {
                    b.HasOne("TopezEventBot.Data.Entities.AccountLink", "AccountLink")
                        .WithMany("EventParticipations")
                        .HasForeignKey("AccountLinkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TopezEventBot.Data.Entities.TrackableEvent", "Event")
                        .WithMany("EventParticipations")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountLink");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.AccountLink", b =>
                {
                    b.Navigation("EventParticipations");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.TrackableEvent", b =>
                {
                    b.Navigation("EventParticipations");
                });
#pragma warning restore 612, 618
        }
    }
}
