using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class alter_sp_GetAllPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPerson = @"
            ALTER PROCEDURE [dbo].[GetAllPerson]
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

        }
    }
}
