using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrustDrop.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    user_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    user_password_hash = table.Column<byte[]>(type: "bytea", maxLength: 32, nullable: false),
                    user_salt = table.Column<byte[]>(type: "bytea", maxLength: 16, nullable: false),
                    user_role = table.Column<int>(type: "integer", nullable: false),
                    user_last_login = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "tenant",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    tenant_owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tenant_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    tenant_deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenant", x => x.tenant_id);
                    table.ForeignKey(
                        name: "FK_tenant_user_tenant_owner_id",
                        column: x => x.tenant_owner_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "policy",
                columns: table => new
                {
                    policy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    policy_tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    policy_allowed_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    policy_allowed_domain = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    policy_max_downloads = table.Column<int>(type: "integer", nullable: true),
                    policy_valid_until = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    policy_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    policy_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    policy_deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_policy", x => x.policy_id);
                    table.ForeignKey(
                        name: "FK_policy_tenant_policy_tenant_id",
                        column: x => x.policy_tenant_id,
                        principalTable: "tenant",
                        principalColumn: "tenant_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_policy_user_policy_allowed_user_id",
                        column: x => x.policy_allowed_user_id,
                        principalTable: "user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "refresh_token",
                columns: table => new
                {
                    refresh_token_id = table.Column<Guid>(type: "uuid", nullable: false),
                    refresh_token_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    refresh_token_tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    refresh_token_hash = table.Column<byte[]>(type: "bytea", maxLength: 64, nullable: false),
                    refresh_token_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    refresh_token_revoked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    refresh_token_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    refresh_token_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    refresh_token_deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_token", x => x.refresh_token_id);
                    table.ForeignKey(
                        name: "FK_refresh_token_tenant_refresh_token_tenant_id",
                        column: x => x.refresh_token_tenant_id,
                        principalTable: "tenant",
                        principalColumn: "tenant_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_refresh_token_user_refresh_token_user_id",
                        column: x => x.refresh_token_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_tenant_role",
                columns: table => new
                {
                    user_tenant_role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_tenant_role_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_tenant_role_tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_tenant_role_role = table.Column<int>(type: "integer", nullable: false),
                    user_tenant_role_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_tenant_role_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_tenant_role_deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_tenant_role", x => x.user_tenant_role_id);
                    table.ForeignKey(
                        name: "FK_user_tenant_role_tenant_user_tenant_role_tenant_id",
                        column: x => x.user_tenant_role_tenant_id,
                        principalTable: "tenant",
                        principalColumn: "tenant_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_tenant_role_user_user_tenant_role_user_id",
                        column: x => x.user_tenant_role_user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "document",
                columns: table => new
                {
                    document_id = table.Column<Guid>(type: "uuid", nullable: false),
                    document_tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    document_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    document_owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    document_policy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    document_size = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    document_hash = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    document_content_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    document_status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    document_storage_key = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    document_scanned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    document_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    document_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    document_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    document_deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document", x => x.document_id);
                    table.ForeignKey(
                        name: "FK_document_policy_document_policy_id",
                        column: x => x.document_policy_id,
                        principalTable: "policy",
                        principalColumn: "policy_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_document_tenant_document_tenant_id",
                        column: x => x.document_tenant_id,
                        principalTable: "tenant",
                        principalColumn: "tenant_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_document_user_document_owner_id",
                        column: x => x.document_owner_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "access_token",
                columns: table => new
                {
                    access_token_id = table.Column<Guid>(type: "uuid", nullable: false),
                    access_token_tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    access_token_document_id = table.Column<Guid>(type: "uuid", nullable: false),
                    access_token_hash = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    access_token_one_time = table.Column<bool>(type: "boolean", nullable: false),
                    access_token_ttl_seconds = table.Column<int>(type: "integer", nullable: false),
                    access_token_used_count = table.Column<int>(type: "integer", nullable: false),
                    access_token_last_used_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    access_token_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    access_token_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    access_token_deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_token", x => x.access_token_id);
                    table.ForeignKey(
                        name: "FK_access_token_document_access_token_document_id",
                        column: x => x.access_token_document_id,
                        principalTable: "document",
                        principalColumn: "document_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_access_token_tenant_access_token_tenant_id",
                        column: x => x.access_token_tenant_id,
                        principalTable: "tenant",
                        principalColumn: "tenant_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audit",
                columns: table => new
                {
                    audit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    audit_tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    audit_actor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    audit_document_id = table.Column<Guid>(type: "uuid", nullable: false),
                    audit_action = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    audit_ip = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    audit_ua = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    audit_details = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    audit_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    audit_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    audit_deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit", x => x.audit_id);
                    table.ForeignKey(
                        name: "FK_audit_document_audit_document_id",
                        column: x => x.audit_document_id,
                        principalTable: "document",
                        principalColumn: "document_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_audit_tenant_audit_tenant_id",
                        column: x => x.audit_tenant_id,
                        principalTable: "tenant",
                        principalColumn: "tenant_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_audit_user_audit_actor_id",
                        column: x => x.audit_actor_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_access_token_access_token_document_id",
                table: "access_token",
                column: "access_token_document_id");

            migrationBuilder.CreateIndex(
                name: "IX_access_token_access_token_tenant_id",
                table: "access_token",
                column: "access_token_tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_audit_actor_id",
                table: "audit",
                column: "audit_actor_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_audit_document_id",
                table: "audit",
                column: "audit_document_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_audit_tenant_id",
                table: "audit",
                column: "audit_tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_document_document_owner_id",
                table: "document",
                column: "document_owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_document_document_policy_id",
                table: "document",
                column: "document_policy_id");

            migrationBuilder.CreateIndex(
                name: "IX_document_document_tenant_id",
                table: "document",
                column: "document_tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_policy_policy_allowed_user_id",
                table: "policy",
                column: "policy_allowed_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_policy_policy_tenant_id",
                table: "policy",
                column: "policy_tenant_id");

            migrationBuilder.CreateIndex(
                name: "index_refresh_token_token_hash_unique",
                table: "refresh_token",
                column: "refresh_token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "index_refresh_token_user_tenant",
                table: "refresh_token",
                columns: new[] { "refresh_token_user_id", "refresh_token_tenant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_refresh_token_refresh_token_tenant_id",
                table: "refresh_token",
                column: "refresh_token_tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_tenant_tenant_owner_id",
                table: "tenant",
                column: "tenant_owner_id");

            migrationBuilder.CreateIndex(
                name: "index_user_email_unique",
                table: "user",
                column: "user_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "index_user_username_unique",
                table: "user",
                column: "user_username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_tenant_role_user_tenant_role_tenant_id",
                table: "user_tenant_role",
                column: "user_tenant_role_tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_tenant_role_user_tenant_role_user_id_user_tenant_role_~",
                table: "user_tenant_role",
                columns: new[] { "user_tenant_role_user_id", "user_tenant_role_tenant_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_token");

            migrationBuilder.DropTable(
                name: "audit");

            migrationBuilder.DropTable(
                name: "refresh_token");

            migrationBuilder.DropTable(
                name: "user_tenant_role");

            migrationBuilder.DropTable(
                name: "document");

            migrationBuilder.DropTable(
                name: "policy");

            migrationBuilder.DropTable(
                name: "tenant");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
