using Domain;

namespace ServiceContracts
{
    public interface IWeatherService
    {
        List<City> GetCities();

        City GetCity(string cityIdCityUniqueCode);
    }
}
