using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using ServiceContract;
using ServiceContract.DTOs;
using Services;
using System.Threading.Tasks;

namespace Tests
{
    public class CountryServiceTest
    {
        private readonly ICountryService _countryService;

        //constructor
        public CountryServiceTest()
        {
            //A Tesztek során használt adatokat (egyedeket) tárolja. DB helyett.
            var countriesInitial = new List<Country>();

            // Mockolt DbContext létrehozása adatbázis kapcsolat nélkül.
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );

            

            // A mockolt DbContext objektum lekérése.
            // Ezt használjuk a tesztben a valódi DbContext helyett.
            ApplicationDbContext DbContext = dbContextMock.Object;

            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitial);

            _countryService = new CountryService(DbContext);

            //Mivel mi azokat az adatokat szeretnénk hasznáni, amit már itt meg is adtunk, ezért nem szeretnénk inicializálni a Mock-oltakat, ezért 0 értéket adunk át.
            //this._countryService = new CountryService(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options));
        }

        #region AddCountry
        //Amikor a CountryAddRequest null, akkor throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act                              
                await this._countryService.AddCountry(countryAddRequest);
            });

            //public CountryResponse AddCountry(CountryAddRequest? countryRequest);
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
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                Name = "Hungary"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                Name = "Hungary"
            };
            //Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await this._countryService.AddCountry(countryAddRequest1);
                await this._countryService.AddCountry(countryAddRequest2);
            });
        }

        //Ha megfelelő a CountryName akkor megfelelő property-vel rendelkező CountryAddResponse objetkumot kapunk
        [Fact]
        public async Task AddCountry_ProperCountry()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                Name = "Hungary"
            };
            //Act
            CountryResponse countryResponse = await this._countryService.AddCountry(countryAddRequest);
            List<CountryResponse> responseList = await this._countryService.GetAllCountries();
            //Assert
            Assert.True(countryResponse.CountryID != Guid.Empty);
            //Ahhoz, hogy a Contains(Equal) helyesen máködjön, deklarálni kell hogy a County objektum mikor lesz egyenlő (Equal override) egy másik county objektummal.
            //Különben referenciát néz.
            Assert.Contains(countryResponse, responseList);
        }
        #endregion

        #region GetAllCountries
        //Ha nem adunk hozzá Country-t, akkor a lista üres.
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            //Act
            List<CountryResponse> acturalCountries = await this._countryService.GetAllCountries();
            //Assert
            Assert.Empty(acturalCountries);

        }

        [Fact]
        public async Task GetAllCountries()
        {
            //Arrange
            List<CountryAddRequest> countryAddRequests = new List<CountryAddRequest>()
            {
                new CountryAddRequest()
                {
                    Name = "Hungary"
                },
                new CountryAddRequest()
                {
                    Name = "Austria"
                }
            };
            //Act
            List<CountryResponse> countryFromService = new List<CountryResponse>();
            foreach (CountryAddRequest countryAddRequest in countryAddRequests)
            {
                countryFromService.Add(await this._countryService.AddCountry(countryAddRequest));
            }

            List<CountryResponse> actualCountryFromService = await this._countryService.GetAllCountries();

            //Assert
            foreach (CountryResponse country in countryFromService)
            {
                //Ahhoz, hogy a Contains(Equal) helyesen máködjön, deklarálni kell hogy a County objektum mikor lesz egyenlő (Equal override) egy másik county objektummal.
                //Különben referenciát néz.
                Assert.Contains(country, actualCountryFromService);
            }
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
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                Name = "Hungary"
            };
            CountryResponse countryResponse = await this._countryService.AddCountry(countryAddRequest);
            //Act            
            CountryResponse? Country_from_GetCountry = await this._countryService.GetCountryById(countryResponse.CountryID);
            //Assert
            Assert.Equal(countryResponse, Country_from_GetCountry);
        }
        #endregion
    }
}
