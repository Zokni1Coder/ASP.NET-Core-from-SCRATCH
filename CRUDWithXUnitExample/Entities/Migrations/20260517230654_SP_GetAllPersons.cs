using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class SP_GetAllPersons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPerson = @"
            CREATE PROCEDURE [dbo].[GetAllPerson]
            AS BEGIN
            SELECT PersonID, PersonName, Email, DateOfBirth, ReceiveNewsLetters, Gender, CountryID, Address, TaxIdentificationNumber
            FROM [dbo].[Persons]
            END
            ";

            migrationBuilder.Sql(sp_GetAllPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Azért kell ez a rész, mert a Down a roll-back funkciót tölti be és ezzel törölni tudjuk a létrehozott procedure-t ha undo-zni szeretnénk az adatbázis módosítást.
            string sp_GetAllPerson = @"
            DROP PROCEDURE [dbo].[GetAllPerson]";

            migrationBuilder.Sql(sp_GetAllPerson);
        }
    }
}
