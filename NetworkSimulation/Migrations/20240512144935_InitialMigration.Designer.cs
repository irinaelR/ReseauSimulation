﻿// <auto-generated />
using Dentisterie.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetworkSimulation.Migrations
{
    [DbContext(typeof(ReseauDbContext))]
    [Migration("20240512144935_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NetworkSimulation.Entities.Serveur", b =>
                {
                    b.Property<int>("idServeur")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("idServeur"));

                    b.Property<bool>("activite")
                        .HasColumnType("boolean");

                    b.Property<string>("ipAdress")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<int>("xPos")
                        .HasColumnType("integer");

                    b.Property<int>("yPos")
                        .HasColumnType("integer");

                    b.HasKey("idServeur");

                    b.ToTable("serveurs");
                });
#pragma warning restore 612, 618
        }
    }
}