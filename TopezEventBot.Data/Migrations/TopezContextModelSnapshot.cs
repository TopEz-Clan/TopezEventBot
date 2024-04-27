﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TopezEventBot.Data.Context;

#nullable disable

namespace TopezEventBot.Data.Migrations
{
    [DbContext(typeof(TopezContext))]
    partial class TopezContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("TopezEventBot.Data.Entities.AccountLink", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("DiscordMemberId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RunescapeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AccountLinks");
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

                    b.Property<string>("Activity")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ScheduledAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SchedulableEvents");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.SchedulableEventParticipation", b =>
                {
                    b.Property<long>("AccountLinkId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("EventId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Notified")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("RegisteredAt")
                        .HasColumnType("TEXT");

                    b.HasKey("AccountLinkId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("SchedulableEventParticipation");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.TrackableEvent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Activity")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("TrackableEvents");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.TrackableEventParticipation", b =>
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

                    b.ToTable("TrackableEventParticipation");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.TrackableEventProgress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FetchedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Progress")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("TrackableEventParticipationAccountLinkId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("TrackableEventParticipationEventId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TrackableEventParticipationAccountLinkId", "TrackableEventParticipationEventId");

                    b.ToTable("TrackableEventProgress");
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

            modelBuilder.Entity("TopezEventBot.Data.Entities.SchedulableEventParticipation", b =>
                {
                    b.HasOne("TopezEventBot.Data.Entities.AccountLink", "AccountLink")
                        .WithMany("SchedulableEventParticipations")
                        .HasForeignKey("AccountLinkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TopezEventBot.Data.Entities.SchedulableEvent", "Event")
                        .WithMany("EventParticipations")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountLink");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.TrackableEventParticipation", b =>
                {
                    b.HasOne("TopezEventBot.Data.Entities.AccountLink", "AccountLink")
                        .WithMany("TrackableEventParticipations")
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

            modelBuilder.Entity("TopezEventBot.Data.Entities.TrackableEventProgress", b =>
                {
                    b.HasOne("TopezEventBot.Data.Entities.TrackableEventParticipation", null)
                        .WithMany("Progress")
                        .HasForeignKey("TrackableEventParticipationAccountLinkId", "TrackableEventParticipationEventId");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.AccountLink", b =>
                {
                    b.Navigation("SchedulableEventParticipations");

                    b.Navigation("TrackableEventParticipations");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.SchedulableEvent", b =>
                {
                    b.Navigation("EventParticipations");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.TrackableEvent", b =>
                {
                    b.Navigation("EventParticipations");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.TrackableEventParticipation", b =>
                {
                    b.Navigation("Progress");
                });
#pragma warning restore 612, 618
        }
    }
}
