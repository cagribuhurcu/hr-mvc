﻿// <auto-generated />
using System;
using HRProject.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HRProject.Repositories.Migrations
{
    [DbContext(typeof(HRProjectContext))]
    [Migration("20230615151940_employeenull-init")]
    partial class employeenullinit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("HRProject.Entities.Entities.Company", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ContractEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ContractStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FoundationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LogoURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MERSISNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaxAdministration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaxNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Title")
                        .HasColumnType("int");

                    b.Property<int?>("TotalEmployees")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("HRProject.Entities.Entities.CompanyManagerEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("BirthPlace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("Department")
                        .HasColumnType("int");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdentificationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPasswordChange")
                        .HasColumnType("bit");

                    b.Property<int?>("JobID")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("QuitDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Role")
                        .HasColumnType("int");

                    b.Property<decimal?>("Salary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SecondLastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CompanyId");

                    b.HasIndex("JobID");

                    b.ToTable("CompanyManagers");
                });

            modelBuilder.Entity("HRProject.Entities.Entities.Employee", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("BirthPlace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("Department")
                        .HasColumnType("int");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdentificationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPasswordChange")
                        .HasColumnType("bit");

                    b.Property<int?>("JobID")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("QuitDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Role")
                        .HasColumnType("int");

                    b.Property<decimal?>("Salary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SecondLastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CompanyId");

                    b.HasIndex("JobID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("HRProject.Entities.Entities.Job", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("Department")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("HRProject.Entities.Entities.SiteManager", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("BirthPlace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Department")
                        .HasColumnType("int");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdentificationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPasswordChange")
                        .HasColumnType("bit");

                    b.Property<int>("JobID")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("QuitDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Role")
                        .HasColumnType("int");

                    b.Property<string>("SecondLastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("JobID");

                    b.ToTable("SiteManagers");
                });

            modelBuilder.Entity("HRProject.Entities.Entities.CompanyManagerEntity", b =>
                {
                    b.HasOne("HRProject.Entities.Entities.Company", "Company")
                        .WithMany("companyManagers")
                        .HasForeignKey("CompanyId");

                    b.HasOne("HRProject.Entities.Entities.Job", "Job")
                        .WithMany("CompanyManagers")
                        .HasForeignKey("JobID");

                    b.Navigation("Company");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("HRProject.Entities.Entities.Employee", b =>
                {
                    b.HasOne("HRProject.Entities.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("HRProject.Entities.Entities.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobID");

                    b.Navigation("Company");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("HRProject.Entities.Entities.SiteManager", b =>
                {
                    b.HasOne("HRProject.Entities.Entities.Job", "Job")
                        .WithMany("siteManagers")
                        .HasForeignKey("JobID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");
                });

            modelBuilder.Entity("HRProject.Entities.Entities.Company", b =>
                {
                    b.Navigation("companyManagers");
                });

            modelBuilder.Entity("HRProject.Entities.Entities.Job", b =>
                {
                    b.Navigation("CompanyManagers");

                    b.Navigation("siteManagers");
                });
#pragma warning restore 612, 618
        }
    }
}
