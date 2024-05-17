﻿// <auto-generated />
using System;
using Event_planning_back.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Event_planning_back.Migrations
{
    [DbContext(typeof(EventPlanningDbContext))]
    [Migration("20240517001056_SetDefaultForIsArchived")]
    partial class SetDefaultForIsArchived
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Event_planning_back.DataAccess.Entities.GuestEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("Event_planning_back.DataAccess.Entities.ProjectEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsArchived")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Event_planning_back.DataAccess.Entities.TaskEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TaskState")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Event_planning_back.DataAccess.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserSurname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Event_planning_back.DataAccess.Entities.UserProjectEntity", b =>
                {
                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProjectsId")
                        .HasColumnType("uuid");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.HasKey("UsersId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("UserProject");
                });

            modelBuilder.Entity("Event_planning_back.DataAccess.Entities.UserTaskEntity", b =>
                {
                    b.Property<Guid>("AssignedUsersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TasksId")
                        .HasColumnType("uuid");

                    b.HasKey("AssignedUsersId", "TasksId");

                    b.HasIndex("TasksId");

                    b.ToTable("UserTask");
                });

            modelBuilder.Entity("Event_planning_back.DataAccess.Entities.GuestEntity", b =>
                {
                    b.HasOne("Event_planning_back.DataAccess.Entities.ProjectEntity", "Project")
                        .WithMany("Guests")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Event_planning_back.DataAccess.Entities.TaskEntity", b =>
                {
                    b.HasOne("Event_planning_back.DataAccess.Entities.ProjectEntity", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Event_planning_back.DataAccess.Entities.UserProjectEntity", b =>
                {
                    b.HasOne("Event_planning_back.DataAccess.Entities.ProjectEntity", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Event_planning_back.DataAccess.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Event_planning_back.DataAccess.Entities.UserTaskEntity", b =>
                {
                    b.HasOne("Event_planning_back.DataAccess.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("AssignedUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Event_planning_back.DataAccess.Entities.TaskEntity", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Event_planning_back.DataAccess.Entities.ProjectEntity", b =>
                {
                    b.Navigation("Guests");

                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
