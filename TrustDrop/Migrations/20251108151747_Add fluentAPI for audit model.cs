using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrustDrop.Migrations
{
    /// <inheritdoc />
    public partial class AddfluentAPIforauditmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_audit_document_audit_document_id",
                table: "audit");

            migrationBuilder.DropForeignKey(
                name: "FK_audit_tenant_audit_tenant_id",
                table: "audit");

            migrationBuilder.DropForeignKey(
                name: "FK_audit_user_audit_actor_id",
                table: "audit");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentModelId",
                table: "audit",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantModelId",
                table: "audit",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserModelId",
                table: "audit",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_audit_DocumentModelId",
                table: "audit",
                column: "DocumentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_TenantModelId",
                table: "audit",
                column: "TenantModelId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_UserModelId",
                table: "audit",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_audit_document_DocumentModelId",
                table: "audit",
                column: "DocumentModelId",
                principalTable: "document",
                principalColumn: "document_id");

            migrationBuilder.AddForeignKey(
                name: "FK_audit_document_audit_document_id",
                table: "audit",
                column: "audit_document_id",
                principalTable: "document",
                principalColumn: "document_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_audit_tenant_TenantModelId",
                table: "audit",
                column: "TenantModelId",
                principalTable: "tenant",
                principalColumn: "tenant_id");

            migrationBuilder.AddForeignKey(
                name: "FK_audit_tenant_audit_tenant_id",
                table: "audit",
                column: "audit_tenant_id",
                principalTable: "tenant",
                principalColumn: "tenant_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_audit_user_UserModelId",
                table: "audit",
                column: "UserModelId",
                principalTable: "user",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_audit_user_audit_actor_id",
                table: "audit",
                column: "audit_actor_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_audit_document_DocumentModelId",
                table: "audit");

            migrationBuilder.DropForeignKey(
                name: "FK_audit_document_audit_document_id",
                table: "audit");

            migrationBuilder.DropForeignKey(
                name: "FK_audit_tenant_TenantModelId",
                table: "audit");

            migrationBuilder.DropForeignKey(
                name: "FK_audit_tenant_audit_tenant_id",
                table: "audit");

            migrationBuilder.DropForeignKey(
                name: "FK_audit_user_UserModelId",
                table: "audit");

            migrationBuilder.DropForeignKey(
                name: "FK_audit_user_audit_actor_id",
                table: "audit");

            migrationBuilder.DropIndex(
                name: "IX_audit_DocumentModelId",
                table: "audit");

            migrationBuilder.DropIndex(
                name: "IX_audit_TenantModelId",
                table: "audit");

            migrationBuilder.DropIndex(
                name: "IX_audit_UserModelId",
                table: "audit");

            migrationBuilder.DropColumn(
                name: "DocumentModelId",
                table: "audit");

            migrationBuilder.DropColumn(
                name: "TenantModelId",
                table: "audit");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "audit");

            migrationBuilder.AddForeignKey(
                name: "FK_audit_document_audit_document_id",
                table: "audit",
                column: "audit_document_id",
                principalTable: "document",
                principalColumn: "document_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_audit_tenant_audit_tenant_id",
                table: "audit",
                column: "audit_tenant_id",
                principalTable: "tenant",
                principalColumn: "tenant_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_audit_user_audit_actor_id",
                table: "audit",
                column: "audit_actor_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
