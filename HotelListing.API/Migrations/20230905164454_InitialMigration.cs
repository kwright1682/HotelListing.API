using Microsoft.EntityFrameworkCore.Migrations;

//////////////////////////////////////////////
//kw - This WHOLE FILE was auto-generated:
//  Tools|Nuget|Package Manager|Package Manager Console:
//
//          PM> add-migration InitialMigration
//  then
//          PM> update-database    The migration (above) has the commands/instructions, and 'update-database' EXECUTES them
//
//  After update-database, go to View|SQL Server Object Explorer AND 'Refresh'
//  After 'Refresh', you will see your HotelListingAPIDb database listed.
//
//  In SQL Server Mangement Studio, you can connect to the newly created database by right clicking
//  'localhost', then select 'Azure Data Studio|New Query' from the popup.
//  This will open Azure Data Studio window INSIDE SQL Server Mgt Studio.
//
//  NOTE: I don't see the new database in native SQL Server Mgt Studio.
//         ??? How to connect to it from there instead of thru Azure Data Studio ???
//////////////////////////////////////////////

#nullable disable

namespace HotelListing.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        /// kw - When I'm doing the migration, this stuff is what I want to do (executed)
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hotels_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade); //kw - Cascade, Restrict...etc...
                });

            //kw - create a sql index on 'CountryId'
            migrationBuilder.CreateIndex(
                name: "IX_Hotels_CountryId",
                table: "Hotels",
                column: "CountryId");
        }

        /// <inheritdoc />
        /// kw - If I want to UNDO the migration, this stuff is what I want to do (executed)
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hotels");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
