using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifyJobEntity3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Clients_ClientId",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Job_JobId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Workers_WorkerId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Job_JobId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offer",
                table: "Offer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Job",
                table: "Job");

            migrationBuilder.RenameTable(
                name: "Offer",
                newName: "Offers");

            migrationBuilder.RenameTable(
                name: "Job",
                newName: "Jobs");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_WorkerId",
                table: "Offers",
                newName: "IX_Offers_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_Job_ClientId",
                table: "Jobs",
                newName: "IX_Jobs_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offers",
                table: "Offers",
                columns: new[] { "JobId", "WorkerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jobs",
                table: "Jobs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Clients_ClientId",
                table: "Jobs",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Jobs_JobId",
                table: "Offers",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Workers_WorkerId",
                table: "Offers",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Jobs_JobId",
                table: "Skills",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Clients_ClientId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Jobs_JobId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Workers_WorkerId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Jobs_JobId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offers",
                table: "Offers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Jobs",
                table: "Jobs");

            migrationBuilder.RenameTable(
                name: "Offers",
                newName: "Offer");

            migrationBuilder.RenameTable(
                name: "Jobs",
                newName: "Job");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_WorkerId",
                table: "Offer",
                newName: "IX_Offer_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_Jobs_ClientId",
                table: "Job",
                newName: "IX_Job_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offer",
                table: "Offer",
                columns: new[] { "JobId", "WorkerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Job",
                table: "Job",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Clients_ClientId",
                table: "Job",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Job_JobId",
                table: "Offer",
                column: "JobId",
                principalTable: "Job",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Workers_WorkerId",
                table: "Offer",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Job_JobId",
                table: "Skills",
                column: "JobId",
                principalTable: "Job",
                principalColumn: "Id");
        }
    }
}
