using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Hozd létre a migrációt "Add-Migration {migration-név}"-vel.
            //a @-al jelölt paraméterek a c#ból jönnek, az anélküliek az adatbázis tábla oszlopainak a nevét.
            //FONTOS, hogy az aktív paramétereket is megjelöljük. Ha az adatázisban PK van, akkor ki kell írni hogy uniqueidentifier vagy ha pl. nvarchar(max), akkor azt is különben update esetén errort kapunk.
            string sp_InsertPerson =
                @"CREATE PROCEDURE [dbo].[InsertPersons] (@PersonID uniqueidentifier, @PersonName nvarchar(40), @Email nvarchar(max), @DateOfBirth datetime2(7), @Gender nvarchar(10), @CountryID uniqueidentifier, @Address nvarchar(40), @ReceiveNewsLetters bit) AS BEGIN 
                  INSERT INTO [dbo].[Persons](PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters)
                  VALUES (@PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters)
                  END";

            migrationBuilder.Sql(sp_InsertPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson =
                @"DROP PROCEDURE [dbo].[InsertPersons]";

            migrationBuilder.Sql(sp_InsertPerson);
        }

        //Amikor megírtad, frissítsd a dbo-t: "Update-Database".
    }
}
