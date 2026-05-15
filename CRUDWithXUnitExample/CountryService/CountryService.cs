using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContract;
using ServiceContract.DTOs;
using System.Runtime.InteropServices;

namespace Services
{
    public class CountryService : ICountryService
    {
        //fieldként elmentjük a contextot, hogy a megfelelő adatbázisra tudjunk hivatkozni.
        private readonly PersonsDbContext _dbContext;
        
        public CountryService(PersonsDbContext personsDbContext)
        {
            this._dbContext = personsDbContext;
        }


        //Így tudsz paraméternek default értéket megadni.
        //public CountryService(bool initialization = true)
        //{
        //    this._countries = new List<Country>();

        //    //Ha a konstruktorban az érték 1, akkor lefutt az inicializáció és ezzel dolgozik. Ha a tesztben is hozzáadunk újabb objektumokat, akkor az is bele lesz téva a listába. A két helyen inicializált objektumok összeadódnak végén. 
        //    if (initialization)
        //    {
        //        _countries.AddRange(new List<Country> { new Country() { CountryID = Guid.Parse("11C64D36-EC2D-4ADE-99F6-469F98E380CF"), CountryName = "Hungary" }, new Country() { CountryID = Guid.Parse("456B9BAD-40EA-4A17-85B3-87C2E5555A26"), CountryName = "Austria" }, new Country() { CountryID = Guid.Parse("B4871C6C-6BB8-4CCF-AA16-CF846D036EDF"), CountryName = "Serbia" }, new Country() { CountryID = Guid.Parse("7ED74F84-21D9-4A9A-A5F2-4390DFD0F40F"), CountryName = "Germany" }, new Country() { CountryID = Guid.Parse("C9CCFE13-E61B-485B-ABCB-B953297C6993"), CountryName = "Italy" }, new Country() { CountryID = Guid.Parse("5716D10D-005A-4347-B27D-F0A50D02279A"), CountryName = "England" } });
        //    }
        //}
        public CountryResponse AddCountry(CountryAddRequest? countryRequest)
        {
            //Ha null a metódus paraméter akkor Exception 
            if (countryRequest is null)
            {
                throw new ArgumentNullException();
            }
            //Ha a Name üres, akkor Exception
            if (countryRequest.Name is null)
            {
                throw new ArgumentException();
            }

            //Alul meghagyom a korábbi verziót összehasonlításnak.
            if (this._dbContext.Countries.Count(country => country.CountryName == countryRequest.Name) > 0)
            {
                throw new Exception("The given Country name is already exists!");
            }

            //if (this._dbContext.Where(x => x.CountryName == countryRequest.Name).Count() > 0)
            //{
            //    throw new Exception("The given Country name is already exists!");
            //}

            //Ahogyláthatod a "Extension" metódus sikeresen hozzá lett addva a Country Entity-hez.
            //Átalakítjuk a countryRequest objketumot Country egyeddé
            Country country = countryRequest.ToCountry();
            //Generálunk neki Guid-t
            country.CountryID = Guid.NewGuid();
            //Hozzáadjuk a belső listához
            this._dbContext.Add(country);
            //Mikor insert történik kötelessek vagyunk elmenteni a változtatást.
            this._dbContext.SaveChanges();

            //Azért célszerű nem a Country egyedet visszaadni és inkább csak a Service-en belül hagyni, hogy kívülről ne legyen látható, csak amit engedünk a CountryResponse-zal.            
            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            List<CountryResponse> countries = this._dbContext.Countries.Select(country => country.ToCountryResponse()).ToList();
            return countries;

            //return (List<CountryResponse>)this._dbContext.Select(x => x.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryById(Guid? countryId)
        {
            if (countryId is null)
            {
                return null;
            }

            //Előbb kiszűri a megfelelő elemeket és uttána kiválasztja az elsőt. 
            //return this._countries.Where(country => country.Guid == countryId).FirstOrDefault().ToCountryResponse();

            //Amint megtalál egy megfelelő elemet, visszaadja.
            //Country? country = this._dbContext.FirstOrDefault(country => country.CountryID == countryId);

            Country? country = this._dbContext.Countries.FirstOrDefault(country => country.CountryID == countryId);
            if (country is null)
            {
                return null;
            }
            return country.ToCountryResponse();
        }


    }
}
