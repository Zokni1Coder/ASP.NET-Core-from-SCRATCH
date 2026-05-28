using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class sp_alters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson =
                @"ALTER PROCEDURE [dbo].[InsertPersons] (@PersonID uniqueidentifier, @PersonName nvarchar(40), @Email nvarchar(max), @DateOfBirth datetime2(7), @Gender nvarchar(10), @CountryID uniqueidentifier, @Address nvarchar(40), @ReceiveNewsLetters bit, @TaxIdentificationNumber varchar(8)) AS BEGIN 
                  INSERT INTO [dbo].[Persons](PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters, TaxIdentificationNumber)
                  VALUES (@PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters, @TaxIdentificationNumber)
                  END";

            migrationBuilder.Sql(sp_InsertPerson);

            string sp_GetPersonByID = @"ALTER PROCEDURE [dbo].[GetPersonByID] (@PersonID uniqueidentifier) AS BEGIN SELECT PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters, TaxIdentificationNumber FROM [dbo].[Persons] WHERE PersonID = @PersonID END";
            
            migrationBuilder.Sql(sp_GetPersonByID);

            string sp_UpdatePerson = @"ALTER PROCEDURE [dbo].[UpdatePerson] (@PersonID uniqueidentifier, @PersonName nvarchar(40), @Email nvarchar(max), @DateOfBirth datetime2(7), @Gender nvarchar(10), @CountryID uniqueidentifier, @Address nvarchar(40), @ReceiveNewsLetters bit, @TaxIdentificationNumber varchar(8)) AS BEGIN UPDATE [dbo].[Persons] SET PersonName = @PersonName, Email = @Email, DateOfBirth = @DateOfBirth, Gender = @Gender, CountryID = @CountryID, Address = @Address, ReceiveNewsLetters = @ReceiveNewsLetters WHERE PersonID = @PersonID END";
            migrationBuilder.Sql(sp_UpdatePerson);


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
