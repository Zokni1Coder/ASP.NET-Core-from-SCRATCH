using ServiceContract.DTOs;

namespace ServiceContract
{
    public interface ICountryService
    {
        /// <summary>
        /// Hozzáad egy Country egy Countries listába.
        /// </summary>
        /// <param name="countryRequest">Country objektum a hozzáadáshoz</param>
        /// <returns>Visszaad egy Country objektumot a hozzáadás után(beleértvbe az újonnan generált Guid-t)</returns>
        public CountryResponse AddCountry(CountryAddRequest? countryRequest);
        /// <summary>
        /// Lekérjük a belső listából az összes Country egyedet 
        /// </summary>
        /// <returns>A Country-kat átalakítjuk CountryResponse-á és vissszadjuk egy listában</returns>
        public List<CountryResponse> GetAllCountries();

        /// <summary>
        /// Lekérünk egy Country egyedet a listából
        /// </summary>
        /// <param name="countryId">Ez alapján keresünk Counrty-t</param>
        /// <returns>Vissza ad egy Country egyedet a megfelelő ID-vel</returns>
        public CountryResponse? GetCountryById(Guid? countryId);        
    }
}
