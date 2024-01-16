using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CombineSample.Migrations
{
    /// <inheritdoc />
    public partial class migrations1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "employeePassword",
                table: "AddEmployee_Details",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "employeePassword",
                table: "AddEmployee_Details");
        }
    }
}
