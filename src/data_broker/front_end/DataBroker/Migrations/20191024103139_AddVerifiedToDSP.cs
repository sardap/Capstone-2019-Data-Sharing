using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBroker.Migrations
{
    public partial class AddVerifiedToDSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Verified",
                table: "DataSharingPolicies",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Verified",
                table: "DataSharingPolicies");
        }
    }
}
