using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContract;
using ServiceContract.DTOs;
using Services;

namespace Tests
{
    public class CountryServiceTest
    {
        private readonly ICountryService _countryService;

        //constructor
        public CountryServiceTest()
        {
            //Mivel mi azokat az adatokat szeretnénk hasznáni, amit már itt meg is adtunk, ezért nem szeretnénk inicializálni a Mock-oltakat, ezért 0 értéket adunk át.
            this._countryService = new CountryService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
        }

        #region AddCountry
        //Amikor a CountryAddRequest null, akkor throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act                              
                this._countryService.AddCountry(countryAddRequest);
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
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                Name = null
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._countryService.AddCountry(countryAddRequest);
            });
        }

        //Amikor a CountryName dupla, akkor throw ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName()
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
            Assert.Throws<Exception>(() =>
            {
                this._countryService.AddCountry(countryAddRequest1);
                this._countryService.AddCountry(countryAddRequest2);
            });
        }

        //Ha megfelelő a CountryName akkor megfelelő property-vel rendelkező CountryAddResponse objetkumot kapunk
        [Fact]
        public void AddCountry_ProperCountry()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                Name = "Hungary"
            };
            //Act
            CountryResponse countryResponse = this._countryService.AddCountry(countryAddRequest);
            List<CountryResponse> responseList = this._countryService.GetAllCountries();
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
        public void GetAllCountries_EmptyList()
        {
            //Act
            List<CountryResponse> acturalCountries = this._countryService.GetAllCountries();
            //Assert
            Assert.Empty(acturalCountries);

        }

        [Fact]
        public void GetAllCountries()
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
                countryFromService.Add(this._countryService.AddCountry(countryAddRequest));
            }

            List<CountryResponse> actualCountryFromService = this._countryService.GetAllCountries();

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
        public void GetCountryByCountryId_CountryIdIsNull()
        {
            //Arrange
            Guid? guid = null;
            //Act
            CountryResponse? countryResponse = this._countryService.GetCountryById(guid);
            //Assert
            Assert.Null(countryResponse);
        }

        //Ha megfelelő paramétert adunk át, akkor a megfelelőt kell megkapnunk.
        [Fact]
        public void GetCountryByCountryId_CountryIdIsProper()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                Name = "Hungary"
            };
            CountryResponse countryResponse = this._countryService.AddCountry(countryAddRequest);
            //Act            
            CountryResponse? Country_from_GetCountry = this._countryService.GetCountryById(countryResponse.CountryID);
            //Assert
            Assert.Equal(countryResponse, Country_from_GetCountry);
        }
        #endregion
    }
}
