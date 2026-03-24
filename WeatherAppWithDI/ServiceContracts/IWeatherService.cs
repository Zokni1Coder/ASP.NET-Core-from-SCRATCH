using Domain;

namespace ServiceContracts
{
    public interface IWeatherService
    {
        List<CityWeather> GetWeatherDetails();

        CityWeather GetCity(string cityIdCityUniqueCode);
    }
}
