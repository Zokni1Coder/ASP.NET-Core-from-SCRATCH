using ServiceContracts;
using Domain;

namespace Services
{
    public class WeatherService : IWeatherService
    {
        private List<CityWeather> _cities = new List<CityWeather>();

        public WeatherService()
        {
            _cities.Add(new CityWeather
            {
                CityUniqueCode = "LDN",
                CityName = "London",
                DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),
                TemperatureFahrenheit = 33
            });
            _cities.Add(new CityWeather
            {
                CityUniqueCode = "NYC",
                CityName = "London",
                DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),
                TemperatureFahrenheit = 60
            });
            _cities.Add(new CityWeather
            {
                CityUniqueCode = "PAR",
                CityName = "Paris",
                DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),
                TemperatureFahrenheit = 82
            });
        }
        public List<CityWeather> GetWeatherDetails()
        {
            return this._cities;
        }

        public CityWeather GetCity(string cityIdCityUniqueCode) => this._cities.Where(id => id.CityUniqueCode == cityIdCityUniqueCode).FirstOrDefault();

    }
}
