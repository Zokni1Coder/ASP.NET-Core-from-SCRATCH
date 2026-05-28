using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class GetPersonByID_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetPersonByID = @"CREATE PROCEDURE [dbo].[GetPersonByID] (@PersonID uniqueidentifier) AS BEGIN SELECT PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters FROM [dbo].[Persons] WHERE PersonID = @PersonID END";

            migrationBuilder.Sql(sp_GetPersonByID);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Azért kell ez a rész, mert a Down a roll-back funkciót tölti be és ezzel törölni tudjuk a létrehozott procedure-t ha undo-zni szeretnénk az adatbázis módosítást.
            string sp_GetPersonByID = @"
            DROP PROCEDURE [dbo].[GetPersonByID]";

            migrationBuilder.Sql(sp_GetPersonByID);
        }
    }
}
