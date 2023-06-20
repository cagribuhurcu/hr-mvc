using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRProject.Repositories.Migrations
{
    public partial class newemppermis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "EmployeePermissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequiredDate",
                table: "EmployeePermissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "EmployeePermissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalRequiredDay",
                table: "EmployeePermissions",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "EmployeePermissions");

            migrationBuilder.DropColumn(
                name: "RequiredDate",
                table: "EmployeePermissions");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "EmployeePermissions");

            migrationBuilder.DropColumn(
                name: "TotalRequiredDay",
                table: "EmployeePermissions");
        }
    }
}
