using Microsoft.EntityFrameworkCore.Migrations;

namespace CTrace.Migrations
{
    public partial class contactupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tab_users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "user_pass", "user_salt" },
                values: new object[] { "UYz2MVl67Qazwvbg8rngOFqCSarndHLEODGMr7NDRpQ=", "J7J1qL0Dnzu/pp/sXM1lpA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tab_users",
                keyColumn: "user_id",
                keyValue: 1,
                columns: new[] { "user_pass", "user_salt" },
                values: new object[] { "ibNJmZ+Lb9vZWKTKCqJzVLgcPsUtW07VDreb52Y8wZE=", "V9lVvNz9j0/grVKmGzC0Pg==" });
        }
    }
}
