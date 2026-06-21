using AutoFixture;
using CRUDWithXUnitExample.Controllers;
using Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiceContract;
using ServiceContract.DTOs;
using ServiceContract.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class PersonsControllerTest
    {
        //Mockolni kell a két service-t, mert a Controller őket fogja hívni.
        private readonly Mock<ICountryService> _countryServiceMock;
        private readonly Mock<IPersonService> _personServiceMock;
        private readonly IPersonService _personService;
        private readonly ICountryService _countryService
            ;
        private readonly IFixture _fixture;

        public PersonsControllerTest()
        {
            this._fixture = new Fixture();
            this._personServiceMock = new Mock<IPersonService>();
            this._countryServiceMock = new Mock<ICountryService>();
            this._countryService = this._countryServiceMock.Object;
            this._personService = this._personServiceMock.Object;
        }

        #region Index

        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            //Arrange
            List<PersonResponse> persons = this._fixture.Create<List<PersonResponse>>();

            this._personServiceMock.Setup(method => method.GetFilteredPerson(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(persons);

            //Az It.IsAny<típus>() jelöli a szükséges paramétereket. 
            //Tehát esetünkben a GetSortedPersons metódus fejléce:  GetSortedPersons(List<PersonResponse> lista, string a, SortingOptions option)
            this._personServiceMock.Setup(method => method.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortingOptions>())).ReturnsAsync(persons);

            PersonsController controller = new PersonsController(this._countryService, this._personService);

            //Act
            IActionResult result = await controller.Index(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SortingOptions>());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            //ViewData.Model-el érjük el a View-nak átadott Model objhektumot. Először a típusát, utána az értékét ellenőrizzük.
            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(persons);
        }
        #endregion

        #region Create

        [Fact]
        public async Task Index_SholdReturnCreate()
        {
            //Arrange
            PersonAddRequest personAddRequest = this._fixture.Build<PersonAddRequest>().Without(prop => prop.PersonName).Without(prop => prop.Email).Create();

            List<CountryResponse> countries = this._fixture.Create<List<CountryResponse>>();

            this._countryServiceMock.Setup(method => method.GetAllCountries()).ReturnsAsync(countries);

            PersonsController controller = new PersonsController(this._countryService, this._personService);

            controller.ModelState.AddModelError("PersonName", "Person should not be null");

            //Act
            IActionResult actionResult = await controller.Create(personAddRequest);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(actionResult);
        }

        [Fact]
        public async Task Index_SholdReturnRedirectToIndex()
        {
            //Arrange
            PersonAddRequest personAddRequest = this._fixture.Build<PersonAddRequest>().Without(prop => prop.PersonName).Without(prop => prop.Email).Create();

            List<CountryResponse> countries = this._fixture.Create<List<CountryResponse>>();

            this._countryServiceMock.Setup(method => method.GetAllCountries()).ReturnsAsync(countries);

            PersonsController controller = new PersonsController(this._countryService, this._personService);

            //Act
            IActionResult actionResult = await controller.Create(personAddRequest);

            //Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(actionResult);

            redirectResult.ActionName.Should().Be("Index");
            redirectResult.ControllerName.Should().Be("persons");

        }

        #endregion
    }
}
