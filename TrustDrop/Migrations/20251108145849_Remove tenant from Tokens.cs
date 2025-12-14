using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrustDrop.Migrations
{
    /// <inheritdoc />
    public partial class RemovetenantfromTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_token_tenant_refresh_token_tenant_id",
                table: "refresh_token");

            migrationBuilder.AlterColumn<Guid>(
                name: "refresh_token_tenant_id",
                table: "refresh_token",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_token_tenant_refresh_token_tenant_id",
                table: "refresh_token",
                column: "refresh_token_tenant_id",
                principalTable: "tenant",
                principalColumn: "tenant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_token_tenant_refresh_token_tenant_id",
                table: "refresh_token");

            migrationBuilder.AlterColumn<Guid>(
                name: "refresh_token_tenant_id",
                table: "refresh_token",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_token_tenant_refresh_token_tenant_id",
                table: "refresh_token",
                column: "refresh_token_tenant_id",
                principalTable: "tenant",
                principalColumn: "tenant_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
