using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBroker.Migrations
{
    public partial class AddPolicyIdToLinking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PolicyId",
                table: "UserTokenLinkings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PolicyId",
                table: "UserTokenLinkings");
        }
    }
}
