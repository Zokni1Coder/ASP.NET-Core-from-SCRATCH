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
    }
}
