using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CTrace.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tab_users",
                keyColumn: "user_id",
                keyValue: new Guid("69c3e099-d215-4b8d-9192-d204f18168a4"));

            migrationBuilder.DropColumn(
                name: "user_fname",
                table: "tab_users");

            migrationBuilder.DropColumn(
                name: "user_lname",
                table: "tab_users");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "tab_users",
                nullable: false,
                defaultValue: new Guid("d5b729e2-18ed-49d4-8948-587f1df44485"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("fe35927f-049a-4768-817b-6808d37113fa"));

            migrationBuilder.AddColumn<string>(
                name: "user_name",
                table: "tab_users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "tab_users",
                columns: new[] { "user_id", "user_isadmin", "user_mobile", "user_name", "user_pass", "user_salt" },
                values: new object[] { new Guid("a07c0078-32d4-43d8-80ea-6f18388b99df"), true, "admin", "Admin", "53pFNdTQOd79WeRf0wQfxJvFBKT1qThh3FeA8FaS6go=", "T1SWFqtQpE8g4SSxecQ5GQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tab_users",
                keyColumn: "user_id",
                keyValue: new Guid("a07c0078-32d4-43d8-80ea-6f18388b99df"));

            migrationBuilder.DropColumn(
                name: "user_name",
                table: "tab_users");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "tab_users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("fe35927f-049a-4768-817b-6808d37113fa"),
                oldClrType: typeof(Guid),
                oldDefaultValue: new Guid("d5b729e2-18ed-49d4-8948-587f1df44485"));

            migrationBuilder.AddColumn<string>(
                name: "user_fname",
                table: "tab_users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "user_lname",
                table: "tab_users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "tab_users",
                columns: new[] { "user_id", "user_fname", "user_isadmin", "user_lname", "user_mobile", "user_pass", "user_salt" },
                values: new object[] { new Guid("69c3e099-d215-4b8d-9192-d204f18168a4"), "Admin", true, null, "admin", "Qosok1wMdRHHX+CntKHYubf1j8eK3h6oxjl34mt/bOM=", "2A/eYfNMF4DKNRzOVRAReQ==" });
        }
    }
}
