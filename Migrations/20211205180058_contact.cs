using Microsoft.EntityFrameworkCore.Migrations;

namespace CTrace.Migrations
{
    public partial class contact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tab_users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "user_pass", "user_salt" },
                values: new object[] { "ibNJmZ+Lb9vZWKTKCqJzVLgcPsUtW07VDreb52Y8wZE=", "V9lVvNz9j0/grVKmGzC0Pg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tab_users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "user_pass", "user_salt" },
                values: new object[] { "cKqBaajdQlg992yozgv8HIQajXgaC8g9kgQlsAP4VkQ=", "+WWMvZ2V1eeXkVnhJ4naHg==" });
        }
    }
}
