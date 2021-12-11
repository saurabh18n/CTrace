using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CTrace.Migrations
{
    public partial class contactupdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tab_contact",
                columns: table => new
                {
                    contact_id = table.Column<Guid>(nullable: false),
                    PrimaryUseruser_id = table.Column<int>(nullable: false),
                    SecondUseruser_id = table.Column<int>(nullable: false),
                    contact_time = table.Column<DateTime>(nullable: false),
                    CreatedByuser_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_contact", x => x.contact_id);
                    table.ForeignKey(
                        name: "FK_tab_contact_tab_users_CreatedByuser_id",
                        column: x => x.CreatedByuser_id,
                        principalTable: "tab_users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_tab_contact_tab_users_PrimaryUseruser_id",
                        column: x => x.PrimaryUseruser_id,
                        principalTable: "tab_users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_tab_contact_tab_users_SecondUseruser_id",
                        column: x => x.SecondUseruser_id,
                        principalTable: "tab_users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "tab_detection",
                columns: table => new
                {
                    detection_id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    detection_time = table.Column<DateTime>(nullable: false),
                    CreatedByuser_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_detection", x => x.detection_id);
                    table.ForeignKey(
                        name: "FK_tab_detection_tab_users_CreatedByuser_id",
                        column: x => x.CreatedByuser_id,
                        principalTable: "tab_users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_tab_detection_tab_users_user_id",
                        column: x => x.user_id,
                        principalTable: "tab_users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "tab_notification",
                columns: table => new
                {
                    notif_id = table.Column<Guid>(nullable: false),
                    notif_useruser_id = table.Column<int>(nullable: false),
                    notif_text = table.Column<string>(nullable: false),
                    notif_redirect = table.Column<string>(nullable: true),
                    notif_created = table.Column<DateTime>(nullable: false),
                    notif_ṛead = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_notification", x => x.notif_id);
                    table.ForeignKey(
                        name: "FK_tab_notification_tab_users_notif_useruser_id",
                        column: x => x.notif_useruser_id,
                        principalTable: "tab_users",
                        principalColumn: "user_id");
                });

            migrationBuilder.UpdateData(
                table: "tab_users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "user_pass", "user_salt" },
                values: new object[] { "7yqhZk6N5bvBDekEL0wq8cL2nbYtZChYu9BJqSLXpuA=", "SxOURuX3iCNyJmm2RpfIHg==" });

            migrationBuilder.CreateIndex(
                name: "IX_tab_contact_CreatedByuser_id",
                table: "tab_contact",
                column: "CreatedByuser_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_contact_PrimaryUseruser_id",
                table: "tab_contact",
                column: "PrimaryUseruser_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_contact_SecondUseruser_id",
                table: "tab_contact",
                column: "SecondUseruser_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_detection_CreatedByuser_id",
                table: "tab_detection",
                column: "CreatedByuser_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_detection_user_id",
                table: "tab_detection",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_notification_notif_useruser_id",
                table: "tab_notification",
                column: "notif_useruser_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tab_contact");

            migrationBuilder.DropTable(
                name: "tab_detection");

            migrationBuilder.DropTable(
                name: "tab_notification");

            migrationBuilder.UpdateData(
                table: "tab_users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "user_pass", "user_salt" },
                values: new object[] { "UYz2MVl67Qazwvbg8rngOFqCSarndHLEODGMr7NDRpQ=", "J7J1qL0Dnzu/pp/sXM1lpA==" });
        }
    }
}
