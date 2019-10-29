using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBroker.Migrations
{
    public partial class DataSharingPolicy_03102019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataSharingPolicies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ExcludedBuyers = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    MinPrice = table.Column<decimal>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSharingPolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTokenLinkings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    PolicyCreationToken = table.Column<string>(type: "varchar(100)", nullable: true),
                    PolicyBlockchainLocation = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokenLinkings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataSharingPolicies");

            migrationBuilder.DropTable(
                name: "UserTokenLinkings");
        }
    }
}
