using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoData.Migrations
{
    /// <inheritdoc />
    public partial class TableAM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyTableAM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    leader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    completeTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    amMachine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    puncherMachine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    heatPuncher = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyTableAM", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyTableAM");
        }
    }
}
