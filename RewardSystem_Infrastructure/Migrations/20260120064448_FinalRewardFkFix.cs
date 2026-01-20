using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RewardSystem_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalRewardFkFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "RewardTransactions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PointsTransactions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_RewardTransactions_RewardId",
                table: "RewardTransactions",
                column: "RewardId");

            migrationBuilder.AddForeignKey(
                name: "FK_RewardTransactions_Rewards_RewardId",
                table: "RewardTransactions",
                column: "RewardId",
                principalTable: "Rewards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RewardTransactions_Rewards_RewardId",
                table: "RewardTransactions");

            migrationBuilder.DropIndex(
                name: "IX_RewardTransactions_RewardId",
                table: "RewardTransactions");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "RewardTransactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PointsTransactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
