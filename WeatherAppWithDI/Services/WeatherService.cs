using ServiceContracts;
using WeatherAppWithDI.Models;

namespace Services
{
    public class WeatherService : IWeatherService
    {
        private List<City> _cities = new List<City>();

        public WeatherService()
        {
            _cities.Add(new City
            {
                CityUniqueCode = "LDN",
                CityName = "London",
                DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),
                TemperatureFahrenheit = 33
            });
            _cities.Add(new City
            {
                CityUniqueCode = "NYC",
                CityName = "London",
                DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),
                TemperatureFahrenheit = 60
            });
            _cities.Add(new City
            {
                CityUniqueCode = "PAR",
                CityName = "Paris",
                DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),
                TemperatureFahrenheit = 82
            });
        }
        public List<City> GetCities()
        {
            return this._cities;
        }

        public City GetCity(string cityIdCityUniqueCode) => this._cities.Where(id => id.CityUniqueCode == cityIdCityUniqueCode).FirstOrDefault();

    }
}
