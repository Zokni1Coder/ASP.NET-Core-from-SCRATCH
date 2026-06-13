using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    //A névben a DbContext végződés nem kötelező, de illendő.
    public class ApplicationDbContext : DbContext
    {
        //Miután a ctor-t megírtuk, le kell buildelni és utána futtatni a migrációs parancsot a konzolban.
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Person> Persons { get; set; }

        /// <summary>
        /// Ezzel tudjuk meghívni a c#-ban a procedure-t. Ez lesz a "triggerje".
        /// </summary>
        /// <returns>Visszaadja az összes Person objektumot</returns>
        public async Task<List<Person>> GetAllPerson()
        {
            return await Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPerson]").ToListAsync();
        }

        public async Task<int> UpdatePerson(Person person)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
            new SqlParameter("@PersonID", person.PersonID),
            new SqlParameter("@PersonName", person.PersonName),
            new SqlParameter("@Email", person.Email),
            new SqlParameter("@DateOfBirth", person.DateOfBirth),
            new SqlParameter("@Gender", person.Gender),
            new SqlParameter("@CountryID", person.CountryID),
            new SqlParameter("@Address", person.Address),
            new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters),
            new SqlParameter("@TaxIdentificationNumber", person.TIN)
            };

            return await Database.ExecuteSqlRawAsync("EXECUTE [dbo].[UpdatePerson] @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters, @TaxIdentificationNumber", sqlParameters);
        }

        public async Task<Person?> GetPersonByID(Guid? guid)
        {
            return await Persons.FromSqlRaw("EXECUTE [dbo].[GetPersonByID] @PersonID", new SqlParameter("@PersonID", guid)).FirstOrDefaultAsync();
        }

        public async Task<int> InsertPerson(Person person)
        {
            //Ezek fogják nekünk behelyettesíteni a migrations-ben a "@"-al ellátott paramok értékét.
            SqlParameter[] sqlParameters = new SqlParameter[] {
            new SqlParameter("@PersonID", person.PersonID),
            new SqlParameter("@PersonName", person.PersonName),
            new SqlParameter("@Email", person.Email),
            new SqlParameter("@DateOfBirth", person.DateOfBirth),
            new SqlParameter("@Gender", person.Gender),
            new SqlParameter("@CountryID", person.CountryID),
            new SqlParameter("@Address", person.Address),
            new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
            };

            //Átadjuk az InsertPerson-nak a paramétereket és futtatjuk.
            return await Database.ExecuteSqlRawAsync("EXECUTE [dbo].[InsertPersons] @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters", sqlParameters);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Ezzel manuálisan mg tudjuk adni az oszlop nevét a db-ben. Alapvetően a property neve lenne. Célszerű mindig megadni a nagyobb controll miatt még ha a név nem is változna.
            modelBuilder.Entity<Country>().ToTable("Countries").ToString();
            modelBuilder.Entity<Person>().ToTable("Persons").ToString();

            //Fluent API
            modelBuilder.Entity<Person>().Property(property => property.TIN).HasColumnName("TaxIdentificationNumber").HasColumnType("varchar(8)").HasDefaultValue("DefualtV");

            modelBuilder.Entity<Person>().ToTable(t => t.HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8"));


            //Fluent Api-val a két tábla közötti reláció leírása.
            //modelBuilder.Entity<Person>(person =>
            //{
            //    person.HasOne<Country>(parent => parent.Country)
            //        .WithMany(child => child.Persons)
            //        .HasForeignKey(child => child.CountryID);
            //});

            //A "15_mappa"-ban megtalálsz két json file-t a person és a country névvel. Ezt húzd be. 
            string? countriesJson = System.IO.File.ReadAllText("countries.json").ToString();
            //Mivel json formátumú, ezért deserializálni kell.
            List<Country>? countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach (Country country in countries)
            {
                //A "HasData"-val adjuk meg a seed adatokat.
                modelBuilder.Entity<Country>().HasData(country);
            }

            string? personsJson = System.IO.File.ReadAllText("persons.json").ToString();
            List<Person>? persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);
            foreach (Person person in persons)
            {
                modelBuilder.Entity<Person>().HasData(person);
            }
        }
    }
}
