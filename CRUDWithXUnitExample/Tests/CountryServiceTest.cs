using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Moq;
using RepositoryContracts;
using ServiceContract;
using ServiceContract.DTOs;
using Services;
using System.Threading.Tasks;

namespace Tests
{
    public class CountryServiceTest
    {
        private readonly ICountryService _countryService;
        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IFixture _fixture;

        //constructor
        public CountryServiceTest()
        {
            //A Tesztek során használt adatokat (egyedeket) tárolja. DB helyett.
            var countriesInitial = new List<Country>();

            this._fixture = new Fixture();

            // Mockolt DbContext létrehozása adatbázis kapcsolat nélkül.
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );

            this._countriesRepositoryMock = new Mock<ICountriesRepository>();

            this._countriesRepository = this._countriesRepositoryMock.Object;

            _countryService = new CountryService(this._countriesRepository);

            //Mivel mi azokat az adatokat szeretnénk hasznáni, amit már itt meg is adtunk, ezért nem szeretnénk inicializálni a Mock-oltakat, ezért 0 értéket adunk át.
            //this._countryService = new CountryService(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options));
        }

        #region AddCountry
        //Amikor a CountryAddRequest null, akkor throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange
            //CountryAddRequest? countryAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act                              
                await this._countryService.AddCountry(null);
            });
        }

        //Helyes átalakítás CountryAddRequest-ről Country egyeddé
        [Fact]
        public void AddCountry_Converting()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                Name = "Test"
            };
            //Act
            Country country = countryAddRequest.ToCountry();
            //Assert
            Assert.Equal(country.CountryName, countryAddRequest.Name);
        }

        //Amikor a CountryName null, akkor throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                Name = null
            };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await this._countryService.AddCountry(countryAddRequest);
            });
        }

        //Amikor a CountryName dupla, akkor throw ArgumentException
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest country = this._fixture.Build<CountryAddRequest>().Create();

            this._countriesRepositoryMock.Setup(method => method.GetCountryByName(It.IsAny<string>())).ReturnsAsync(country.ToCountry());

            Func<Task> action = async () =>
            {
                await this._countryService.AddCountry(country);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //Ha megfelelő a CountryName akkor megfelelő property-vel rendelkező CountryAddResponse objetkumot kapunk
        [Fact]
        public async Task AddCountry_ProperCountry()
        {
            //Arrange
            CountryAddRequest addRequest = this._fixture.Build<CountryAddRequest>().Create();

            this._countriesRepositoryMock.Setup(method => method.GetCountryByName(It.IsAny<string>())).ReturnsAsync(null as Country);
            //Act
            CountryResponse result = await this._countryService.AddCountry(addRequest);

            CountryResponse expected = addRequest.ToCountry().ToCountryResponse();

            //Assert
            result.Name.Should().Be(expected.Name);
        }
        #endregion

        #region GetAllCountries
        //Ha nem adunk hozzá Country-t, akkor a lista üres.
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            //Arrange
            this._countriesRepositoryMock.Setup(method => method.GetAllCountries()).ReturnsAsync(new List<Country>());
            //Act
            List<CountryResponse> result = await this._countryService.GetAllCountries();
            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllCountries()
        {
            //Arrange
            List<Country> countries = new List<Country>();
            countries.Add(this._fixture.Build<Country>().Without(prop => prop.Persons).Create());
            countries.Add(this._fixture.Build<Country>().Without(prop => prop.Persons).Create());
            countries.Add(this._fixture.Build<Country>().Without(prop => prop.Persons).Create());

            this._countriesRepositoryMock.Setup(method => method.GetAllCountries()).ReturnsAsync(countries);
            List<CountryResponse> expected = countries.Select(country => country.ToCountryResponse()).ToList();
            //Act
            List<CountryResponse> result = await this._countryService.GetAllCountries();

            //Assert
            result.Should().BeEqualTo(expected);
        }
        #endregion

        #region GetCountryByCountryId
        //Ha null értéket adunk át paraméterként, akkor null értéket kell kapnunk. 
        [Fact]
        public async Task GetCountryByCountryId_CountryIdIsNullAsync()
        {
            //Arrange
            Guid? guid = null;
            //Act
            CountryResponse? countryResponse = await this._countryService.GetCountryById(guid);
            //Assert
            Assert.Null(countryResponse);
        }

        //Ha megfelelő paramétert adunk át, akkor a megfelelőt kell megkapnunk.
        [Fact]
        public async Task GetCountryByCountryId_CountryIdIsProper()
        {
            //Arrange
            Country country = this._fixture.Build<Country>().Without(prop => prop.Persons).Create();

            CountryResponse expected = country.ToCountryResponse();

            this._countriesRepositoryMock.Setup(method => method.GetCountryById(It.IsAny<Guid>())).ReturnsAsync(country);
            //Act            
            CountryResponse? result = await this._countryService.GetCountryById(country.CountryID);
            //Assert
            result.Should().Be(expected);
        }
        #endregion
    }
}
