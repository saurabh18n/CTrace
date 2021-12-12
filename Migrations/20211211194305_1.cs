using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CTrace.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tab_users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(nullable: false, defaultValue: new Guid("fe35927f-049a-4768-817b-6808d37113fa")),
                    user_fname = table.Column<string>(nullable: false),
                    user_lname = table.Column<string>(nullable: true),
                    user_mobile = table.Column<string>(maxLength: 10, nullable: false),
                    user_isadmin = table.Column<bool>(nullable: false),
                    user_salt = table.Column<string>(maxLength: 64, nullable: false),
                    user_pass = table.Column<string>(maxLength: 64, nullable: false),
                    user_failcount = table.Column<byte>(nullable: false, defaultValueSql: "0"),
                    user_lastlogin = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "tab_contact",
                columns: table => new
                {
                    contact_id = table.Column<Guid>(nullable: false),
                    contact_primaryuserid = table.Column<Guid>(nullable: false),
                    contact_seconderyuserid = table.Column<Guid>(nullable: false),
                    contact_time = table.Column<DateTime>(nullable: false),
                    contact_reported = table.Column<DateTime>(nullable: false),
                    contact_createdbyid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_contact", x => x.contact_id);
                    table.ForeignKey(
                        name: "FK_tab_contact_tab_users_contact_createdbyid",
                        column: x => x.contact_createdbyid,
                        principalTable: "tab_users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_tab_contact_tab_users_contact_primaryuserid",
                        column: x => x.contact_primaryuserid,
                        principalTable: "tab_users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_tab_contact_tab_users_contact_seconderyuserid",
                        column: x => x.contact_seconderyuserid,
                        principalTable: "tab_users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "tab_detection",
                columns: table => new
                {
                    detection_id = table.Column<Guid>(nullable: false),
                    detection_user = table.Column<Guid>(nullable: false),
                    detection_time = table.Column<DateTime>(nullable: false),
                    contact_createdby = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_detection", x => x.detection_id);
                    table.ForeignKey(
                        name: "FK_tab_detection_tab_users_contact_createdby",
                        column: x => x.contact_createdby,
                        principalTable: "tab_users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_tab_detection_tab_users_detection_user",
                        column: x => x.detection_user,
                        principalTable: "tab_users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "tab_notification",
                columns: table => new
                {
                    notif_id = table.Column<Guid>(nullable: false),
                    notif_user = table.Column<Guid>(nullable: false),
                    notif_text = table.Column<string>(nullable: false),
                    notif_redirect = table.Column<string>(nullable: true),
                    notif_created = table.Column<DateTime>(nullable: false),
                    notif_ṛead = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_notification", x => x.notif_id);
                    table.ForeignKey(
                        name: "FK_tab_notification_tab_users_notif_user",
                        column: x => x.notif_user,
                        principalTable: "tab_users",
                        principalColumn: "user_id");
                });

            migrationBuilder.InsertData(
                table: "tab_users",
                columns: new[] { "user_id", "user_fname", "user_isadmin", "user_lname", "user_mobile", "user_pass", "user_salt" },
                values: new object[] { new Guid("69c3e099-d215-4b8d-9192-d204f18168a4"), "Admin", true, null, "admin", "Qosok1wMdRHHX+CntKHYubf1j8eK3h6oxjl34mt/bOM=", "2A/eYfNMF4DKNRzOVRAReQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_tab_contact_contact_createdbyid",
                table: "tab_contact",
                column: "contact_createdbyid");

            migrationBuilder.CreateIndex(
                name: "IX_tab_contact_contact_primaryuserid",
                table: "tab_contact",
                column: "contact_primaryuserid");

            migrationBuilder.CreateIndex(
                name: "IX_tab_contact_contact_seconderyuserid",
                table: "tab_contact",
                column: "contact_seconderyuserid");

            migrationBuilder.CreateIndex(
                name: "IX_tab_detection_contact_createdby",
                table: "tab_detection",
                column: "contact_createdby");

            migrationBuilder.CreateIndex(
                name: "IX_tab_detection_detection_user",
                table: "tab_detection",
                column: "detection_user");

            migrationBuilder.CreateIndex(
                name: "IX_tab_notification_notif_user",
                table: "tab_notification",
                column: "notif_user");

            migrationBuilder.CreateIndex(
                name: "IX_tab_users_user_mobile",
                table: "tab_users",
                column: "user_mobile",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tab_contact");

            migrationBuilder.DropTable(
                name: "tab_detection");

            migrationBuilder.DropTable(
                name: "tab_notification");

            migrationBuilder.DropTable(
                name: "tab_users");
        }
    }
}
