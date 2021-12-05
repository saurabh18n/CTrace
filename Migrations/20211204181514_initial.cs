using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CTrace.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tab_users",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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

            migrationBuilder.InsertData(
                table: "tab_users",
                columns: new[] { "user_id", "user_fname", "user_isadmin", "user_lname", "user_mobile", "user_pass", "user_salt" },
                values: new object[] { 1, "Admin", true, null, "admin", "cKqBaajdQlg992yozgv8HIQajXgaC8g9kgQlsAP4VkQ=", "+WWMvZ2V1eeXkVnhJ4naHg==" });

            migrationBuilder.CreateIndex(
                name: "IX_tab_users_user_mobile",
                table: "tab_users",
                column: "user_mobile",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tab_users");
        }
    }
}
