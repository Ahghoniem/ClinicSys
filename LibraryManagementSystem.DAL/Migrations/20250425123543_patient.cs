using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class patient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SId",
                table: "Patient_schedule",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patient_schedule_SId",
                table: "Patient_schedule",
                column: "SId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_schedule_Clinic_schedule_SId",
                table: "Patient_schedule",
                column: "SId",
                principalTable: "Clinic_schedule",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patient_schedule_Clinic_schedule_SId",
                table: "Patient_schedule");

            migrationBuilder.DropIndex(
                name: "IX_Patient_schedule_SId",
                table: "Patient_schedule");

            migrationBuilder.DropColumn(
                name: "SId",
                table: "Patient_schedule");
        }
    }
}
