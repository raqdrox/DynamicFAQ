﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;

namespace FaqSystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220622175448_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Oracle:ValueGenerationStrategy", OracleValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            modelBuilder.Entity("FaqSystem.Models.FaqArticle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Contents")
                        .HasColumnType("clob");

                    b.HasKey("Id");

                    b.ToTable("FaqArticle");
                });

            modelBuilder.Entity("FaqSystem.Models.FaqQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ArticleId");

                    b.Property<int?>("FaqSectionId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("FaqSectionId");

                    b.ToTable("FaqQuestion");
                });

            modelBuilder.Entity("FaqSystem.Models.FaqSection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SectionTitle")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("FaqSection");
                });

            modelBuilder.Entity("FaqSystem.Models.FaqQuestion", b =>
                {
                    b.HasOne("FaqSystem.Models.FaqArticle", "Article")
                        .WithMany()
                        .HasForeignKey("ArticleId");

                    b.HasOne("FaqSystem.Models.FaqSection")
                        .WithMany("QList")
                        .HasForeignKey("FaqSectionId");
                });
#pragma warning restore 612, 618
        }
    }
}
