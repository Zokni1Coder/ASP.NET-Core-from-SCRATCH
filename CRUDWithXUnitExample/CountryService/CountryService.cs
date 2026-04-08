using Entities;
using ServiceContract;
using ServiceContract.DTOs;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly List<Country> _countries;

        public CountryService()
        {
            this._countries = new List<Country>();
        }
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

            if (this._countries.Where(x => x.Name == countryRequest.Name).Count() > 0)
            {
                 throw new Exception("The given Country name is already exists!");
            }

            //Ahogyláthatod a "Extension" metódus sikeresen hozzá lett addva a Country Entity-hez.
            //Átalakítjuk a countryRequest objketumot Country egyeddé
            Country country = countryRequest.ToCountry();
            //Generálunk neki Guid-t
            country.Guid = Guid.NewGuid();
            //Hozzáadjuk a belső listához
            this._countries.Add(country);

            //Azért célszerű nem a Country egyedet visszaadni és inkább csak a Service-en belül hagyni, hogy kívülről ne legyen látható, csak amit engedünk a CountryResponse-zal.            
            return country.ToCountryResponse();
        }
    }
}
