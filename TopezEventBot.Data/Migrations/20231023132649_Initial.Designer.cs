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
    [Migration("20231023132649_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("RunescapeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AccountLinks");
                });

            modelBuilder.Entity("TopezEventBot.Data.Entities.Event", b =>
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

                    b.ToTable("Events");
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

            modelBuilder.Entity("TopezEventBot.Data.Entities.EventParticipation", b =>
                {
                    b.HasOne("TopezEventBot.Data.Entities.AccountLink", "AccountLink")
                        .WithMany("EventParticipations")
                        .HasForeignKey("AccountLinkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TopezEventBot.Data.Entities.Event", "Event")
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

            modelBuilder.Entity("TopezEventBot.Data.Entities.Event", b =>
                {
                    b.Navigation("EventParticipations");
                });
#pragma warning restore 612, 618
        }
    }
}