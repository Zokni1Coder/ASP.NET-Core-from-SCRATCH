using Entities;
using ServiceContract;
using ServiceContract.DTOs;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class CountryServiceTest
    {
        private readonly ICountryService _countryService;

        public CountryServiceTest()
        {
            this._countryService = new CountryService();
        }

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
            Assert.Equal(country.Name, countryAddRequest.Name);
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
            Assert.Throws <Exception> (() =>
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
            CountryResponse countryResponse =  this._countryService.AddCountry(countryAddRequest);
            //Assert
            Assert.True(countryResponse.CountryID != Guid.Empty);
        }

        //Ha minden adat rendben, akkor adja hozzá a Country entity-t a listához
        [Fact]
        public void AddCountry_AddToList()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                Name = "Hungary"
            };
            //Act
            this._countryService.AddCountry(countryAddRequest);
            //Assert
            //Assert.Contains<Country>(this._countryService.);
        }
    }
}
