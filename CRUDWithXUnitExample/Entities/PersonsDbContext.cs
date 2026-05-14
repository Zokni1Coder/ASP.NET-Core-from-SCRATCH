using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    //A névben a DbContext végződés nem kötelező, de illendő.
    public class PersonsDbContext : DbContext
    {
        //Miután a ctor-t megírtuk, le kell buildelni és utána futtatni a migrációs parancsot a konzolban.
        public PersonsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Ezzel manuálisan mg tudjuk adni az oszlop nevét a db-ben. Alapvetően a property neve lenne. Célszerű mindig megadni a nagyobb controll miatt még ha a név nem is változna.
            modelBuilder.Entity<Country>().ToTable("Countries").ToString();
            modelBuilder.Entity<Person>().ToTable("Persons").ToString();

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
