using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRProject.Repositories.Migrations
{
    public partial class jobidandcompanyidnullinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyManagers_Companies_CompanyId",
                table: "CompanyManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyManagers_Jobs_JobID",
                table: "CompanyManagers");

            migrationBuilder.AlterColumn<int>(
                name: "JobID",
                table: "CompanyManagers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "CompanyManagers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyManagers_Companies_CompanyId",
                table: "CompanyManagers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyManagers_Jobs_JobID",
                table: "CompanyManagers",
                column: "JobID",
                principalTable: "Jobs",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyManagers_Companies_CompanyId",
                table: "CompanyManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyManagers_Jobs_JobID",
                table: "CompanyManagers");

            migrationBuilder.AlterColumn<int>(
                name: "JobID",
                table: "CompanyManagers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "CompanyManagers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyManagers_Companies_CompanyId",
                table: "CompanyManagers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyManagers_Jobs_JobID",
                table: "CompanyManagers",
                column: "JobID",
                principalTable: "Jobs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
