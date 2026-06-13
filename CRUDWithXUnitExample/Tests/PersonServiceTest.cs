using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContract;
using ServiceContract.DTOs;
using ServiceContract.Enums;
using Services;
using System;
using System.Threading.Tasks;
using Xunit.Abstractions;
using FluentAssertions;
using FluentAssertions.Specialized;

namespace Tests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        //Adjuk hozzá a DI-t alkalmazva az ITestOutputHelper Interface-t.
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            //ApplicationDbContext personsDbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            //this._countryService = new CountryService(personsDbContext);
            //this._personService = new PersonService(_countryService, personsDbContext);
            _fixture = new Fixture();
            this._testOutputHelper = testOutputHelper;

            var countriesInitial = new List<Country>();
            var personsInitial = new List<Person>();

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );

            ApplicationDbContext DbContext = dbContextMock.Object;

            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitial);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitial);

            _countryService = new CountryService(null);
            _personService = new PersonService(this._countryService, null);
        }

        #region AddPerson
        /// <summary>
        /// Ha null-t adunk át paraméterül, akkor ArgumentNullException kell hogy legyen
        /// </summary>
        [Fact]
        public async Task AddPerson_AddNullValue()
        {
            //Act
            Func<Task> action = async () =>
            {
                await this._personService.AddPerson(null);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        /// <summary>
        /// Ha az PersonAddRequest Name értéke null, akkor ArgumentException kell hogy legyen
        /// </summary>
        [Fact]
        public async void AddPerson_PersonAddRequestNameIsNull()
        {
            //Arrange
            //PersonAddRequest personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = null
            //};
            PersonAddRequest personAddRequest = this._fixture.Build<PersonAddRequest>().With(prop => prop.PersonName, null as string).Create();
            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act:
            //    await this._personService.AddPerson(personAddRequest);
            //});

            Func<Task> action = async () =>
            {
                await this._personService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// Ha megfelelő Person-t adunk hozzá, akkor bele kell hogy kerüljön a listába és egy PersonResponse objektumot kell hogy visszaadjon az újonnan generált PersonId-val.
        /// </summary>
        [Fact]
        public async Task AddPerson_ProperPerson()
        {
            //Arrange
            //PersonAddRequest personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "Reka",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Male,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = Guid.NewGuid()
            //};

            //A fenti manuális teszt objektum létrehozását kiváltjuk a generálással. Azért kell a .Build().With(), mert validáláskor errort kapunk ha ezt nem alkalmazzuk. Miért?
            //Mert ezek nélkül az Email (string) az valahogy így nézne ki és nem passzolna a sablonunkhoz: "Email1235-as12-asd5-qwer6-qwer5"
            //Ha nem lenne email, akkor simán csak a Create() kell a Build().With() nélkül.
            PersonAddRequest personAddRequest = this._fixture.Build<PersonAddRequest>().With(person => person.Email, "someone@gmail.com").Create();

            //Act
            PersonResponse personResponse_from_Add = await this._personService.AddPerson(personAddRequest);
            List<PersonResponse>? personsList = await this._personService.GetAllPersons();

            //Asserts
            //Assert.True(personResponse_from_Add.PersonId != Guid.Empty);
            personResponse_from_Add.PersonId.Should().NotBe(Guid.Empty);

            //Assert.Equal(personResponse_from_Add.ReceiveNewsLetter, personsList[0].ReceiveNewsLetter);
            //Assert.Contains(personResponse_from_Add, await this._personService.GetAllPersons());
            personsList.Should().Contain(personResponse_from_Add);
        }

        //Ha helytelen az emailcím, akkor InvalidOperationException kell hogy legyen.
        //[Fact]
        //public void AddPerson_InCorrectEmail()
        //{
        //    //Arrange
        //    PersonAddRequest personAddRequest = new PersonAddRequest()
        //    {
        //        PersonName = "Reka",
        //        Email = "asdffads.gmail.com" 
        //    };
        //    //Assert
        //    Assert.Throws<InvalidOperationException>(() =>
        //    {
        //        this._personService.AddPerson(personAddRequest);
        //    });
        //}
        #endregion

        #region GetPersonById

        /// <summary>
        /// Ha null paramétert kap, akkor ArgumentNullException kivétel kell hogy dobódjon
        /// </summary>
        [Fact]
        public async Task GetPersonById_EmptyArgument()
        {
            //Arrange
            Guid? guid = null;
            //Assert
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    //Act
            //    await this._personService.GetPersonById(guid);
            //});

            Func<Task> action = async () =>
            {
                await this._personService.GetPersonById(guid);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //Ha megfelelő guid-t adunk át, akkor megtalálja és visszaadja PersonResponse objektumként
        [Fact]
        public async Task GetPersonById_ProperArgument()
        {
            //Arrange
            //CountryAddRequest countryAddRequest = new CountryAddRequest()
            //{
            //    Name = "Hungary",
            //};
            CountryAddRequest countryAddRequest = this._fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse_from_AddCountry = await this._countryService.AddCountry(countryAddRequest);

            //PersonAddRequest request = new PersonAddRequest()
            //{
            //    PersonName = "Reka",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = DateTime.Parse("2005-05-18"),
            //    Gender = Gender.Male,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = countryResponse_from_AddCountry.CountryID
            //};
            PersonAddRequest request = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse_from_AddCountry.CountryID).Create();

            //Act:
            PersonResponse person_from_AddPerson = await this._personService.AddPerson(request);
            PersonResponse? person_from_GetPersonById = await this._personService.GetPersonById(person_from_AddPerson.PersonId);

            //Assert
            //Assert.Equal(person_from_AddPerson, person_from_GetPersonById);
            person_from_GetPersonById.Should().Be(person_from_AddPerson);
        }

        //Ha olyan id-t adunk meg, ami nem létezik, akkor a visszatérési objektum null lesz.
        [Fact]
        public async Task GetPersonById_DoesntExistingGuidInTheListArgument()
        {
            //Arrange
            //PersonAddRequest request = new PersonAddRequest()
            //{
            //    PersonName = "Reka",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Male,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = Guid.NewGuid()
            //};
            PersonAddRequest request = this._fixture.Create<PersonAddRequest>();
            //Act:
            PersonResponse person_from_AddPerson = await this._personService.AddPerson(request);
            PersonResponse? person_from_GetPersonById = new PersonResponse();
            person_from_GetPersonById = await this._personService.GetPersonById(Guid.NewGuid());

            //Assert
            //Assert.Null(person_from_GetPersonById);
            person_from_GetPersonById.Should().BeNull();
        }

        #endregion

        #region GetAllPerson

        //Az elején üres listát kell hogy visszaadjon.
        //Itt a Mock inicializálás miatt nem fog sikeres lenni.
        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse>? persons = new List<PersonResponse>();
            persons = await this._personService.GetAllPersons();

            //Assert
            //Assert.Empty(persons);
            persons.Should().BeEmpty();
        }

        //Megfelelő elemeket ad vissza a lekérdezés.
        [Fact]
        public async Task GetAllPersons_ProperElements()
        {
            //Arrange
            //CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            //{
            //    Name = "Hungary"
            //};
            //CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            //{
            //    Name = "Austria"
            //};
            CountryAddRequest countryAddRequest1 = this._fixture.Create<CountryAddRequest>();
            CountryAddRequest countryAddRequest2 = this._fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await this._countryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await this._countryService.AddCountry(countryAddRequest2);

            //PersonAddRequest personAddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "Reka",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Female,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = countryResponse1.CountryID
            //};
            //PersonAddRequest personAddRequest2 = new PersonAddRequest()
            //{
            //    PersonName = "Reka2",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(2000, 09, 22),
            //    Gender = Gender.Male,
            //    Address = "asd 22.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse2.CountryID
            //};
            PersonAddRequest personAddRequest1 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest2 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse2.CountryID).Create();

            //Act
            PersonResponse personResponse1 = await this._personService.AddPerson(personAddRequest1);
            PersonResponse personResponse2 = await this._personService.AddPerson(personAddRequest2);

            //Pont ugyanúgy kell kiíratni a számunkra fontos adatokat, mint Console projektekben.
            //Expected:
            this._testOutputHelper.WriteLine("Expected:");
            List<PersonResponse> person_From_AddPerson = new List<PersonResponse>() { personResponse1, personResponse2 };

            foreach (PersonResponse person in person_From_AddPerson)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }


            //Actual:
            this._testOutputHelper.WriteLine("Actual:");
            List<PersonResponse>? persons_from_GetAllPersons = await this._personService.GetAllPersons();

            foreach (PersonResponse person in persons_from_GetAllPersons)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Asssert
            foreach (PersonResponse person in person_From_AddPerson)
            {
                //Assert.Contains(person, persons_from_GetAllPersons);
                persons_from_GetAllPersons.Should().Contain(person);
            }
        }

        #endregion

        #region GetFileteredPerson

        //Ha null argumentumokat adunk át keresési értéknek, akkor adja vissza az összeset 
        [Fact]
        public async Task GetFilteredPerson_EmptyArgument()
        {
            //Arrange
            //CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            //{
            //    Name = "Hungary"
            //};
            //CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            //{
            //    Name = "Austria"
            //};
            CountryAddRequest countryAddRequest1 = this._fixture.Create<CountryAddRequest>();
            CountryAddRequest countryAddRequest2 = this._fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await this._countryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await this._countryService.AddCountry(countryAddRequest2);

            //PersonAddRequest personAddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "Reka",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Female,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = countryResponse1.CountryID
            //};
            //PersonAddRequest personAddRequest2 = new PersonAddRequest()
            //{
            //    PersonName = "Reka2",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(2000, 09, 22),
            //    Gender = Gender.Male,
            //    Address = "asd 22.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse2.CountryID
            //};
            //PersonAddRequest personAddRequest3 = new PersonAddRequest()
            //{
            //    PersonName = "Reka3",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(1996, 09, 17),
            //    Gender = Gender.Female,
            //    Address = "asd 30.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse2.CountryID
            //};
            //PersonAddRequest personAddRequest4 = new PersonAddRequest()
            //{
            //    PersonName = "Reka4",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(1971, 12, 05),
            //    Gender = Gender.Male,
            //    Address = "asd 50.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse1.CountryID
            //};
            PersonAddRequest personAddRequest1 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest2 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse2.CountryID).Create();
            PersonAddRequest personAddRequest3 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse2.CountryID).Create();
            PersonAddRequest personAddRequest4 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse1.CountryID).Create();

            PersonResponse personResponse1 = await this._personService.AddPerson(personAddRequest1);
            PersonResponse personResponse2 = await this._personService.AddPerson(personAddRequest2);
            PersonResponse personResponse3 = await this._personService.AddPerson(personAddRequest3);
            PersonResponse personResponse4 = await this._personService.AddPerson(personAddRequest4);

            //Act
            List<PersonResponse>? person_from_GetAll = await this._personService.GetAllPersons();
            this._testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in person_from_GetAll)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            List<PersonResponse>? persons_from_GetFiltered = await this._personService.GetFilteredPerson("CountryId", null);
            this._testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in persons_from_GetFiltered)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            //Assert.Equal(person_from_GetAll, persons_from_GetFiltered);
            persons_from_GetFiltered.Should().BeEqualTo(person_from_GetAll);
        }


        //Ha megfelelő paramétereket adunk át, akkor visszaadja a megfelelő Objektumokat.
        [Fact]
        public async Task GetFilteredPerson_ProperArguments()
        {
            //Arrange
            //CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            //{
            //    Name = "Hungary"
            //};
            //CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            //{
            //    Name = "Austria"
            //};
            CountryAddRequest countryAddRequest1 = this._fixture.Create<CountryAddRequest>();
            CountryAddRequest countryAddRequest2 = this._fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await this._countryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await this._countryService.AddCountry(countryAddRequest2);

            //PersonAddRequest personAddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "Reka",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Female,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = countryResponse1.CountryID
            //};
            //PersonAddRequest personAddRequest2 = new PersonAddRequest()
            //{
            //    PersonName = "Reka2",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(2000, 09, 22),
            //    Gender = Gender.Male,
            //    Address = "asd 22.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse2.CountryID
            //};
            //PersonAddRequest personAddRequest3 = new PersonAddRequest()
            //{
            //    PersonName = "Reka3",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(1996, 09, 17),
            //    Gender = Gender.Female,
            //    Address = "asd 30.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse2.CountryID
            //};
            //PersonAddRequest personAddRequest4 = new PersonAddRequest()
            //{
            //    PersonName = "Reka4",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(1971, 12, 05),
            //    Gender = Gender.Male,
            //    Address = "asd 50.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse1.CountryID
            //};
            PersonAddRequest personAddRequest1 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest2 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest3 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse2.CountryID).Create();
            PersonAddRequest personAddRequest4 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse2.CountryID).Create();

            PersonResponse personResponse1 = await this._personService.AddPerson(personAddRequest1);
            PersonResponse personResponse2 = await this._personService.AddPerson(personAddRequest2);
            PersonResponse personResponse3 = await this._personService.AddPerson(personAddRequest3);
            PersonResponse personResponse4 = await this._personService.AddPerson(personAddRequest4);

            //Act
            List<PersonResponse> person_from_GetFiltered = await this._personService.GetFilteredPerson("CountryId", countryResponse1.CountryID.ToString());
            this._testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person in person_from_GetFiltered)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }
            List<PersonResponse>? AllPersons = await this._personService.GetAllPersons();

            List<PersonResponse>? filtered_from_AddPerson = AllPersons?.Where(person => person.CountryId == countryResponse1.CountryID).ToList();

            this._testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in filtered_from_AddPerson)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            //Assert.Equal(filtered_from_AddPerson, person_from_GetFiltered);
            filtered_from_AddPerson.Should().BeEqualTo(person_from_GetFiltered);
        }

        //Ha megfelelő paramétereket adunk át, akkor visszaadja a megfelelő Objektumokat.
        [Fact]
        public async Task GetFilteredPerson_ProperArgumentsName()
        {
            //Arrange
            //CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            //{
            //    Name = "Hungary"
            //};
            //CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            //{
            //    Name = "Austria"
            //};

            CountryAddRequest countryAddRequest1 = this._fixture.Create<CountryAddRequest>();
            CountryAddRequest countryAddRequest2 = this._fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await this._countryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await this._countryService.AddCountry(countryAddRequest2);

            //PersonAddRequest personAddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "Reka",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Female,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = countryResponse1.CountryID
            //};
            //PersonAddRequest personAddRequest2 = new PersonAddRequest()
            //{
            //    PersonName = "Reka2",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(2000, 09, 22),
            //    Gender = Gender.Male,
            //    Address = "asd 22.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse2.CountryID
            //};
            //PersonAddRequest personAddRequest3 = new PersonAddRequest()
            //{
            //    PersonName = "Reka3",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(1996, 09, 17),
            //    Gender = Gender.Female,
            //    Address = "asd 30.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse2.CountryID
            //};
            //PersonAddRequest personAddRequest4 = new PersonAddRequest()
            //{
            //    PersonName = "Reka4",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(1971, 12, 05),
            //    Gender = Gender.Male,
            //    Address = "asd 50.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse1.CountryID
            //};
            PersonAddRequest personAddRequest1 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse1.CountryID).With(prop => prop.PersonName, "Reka2").Create();
            PersonAddRequest personAddRequest2 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest3 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse2.CountryID).Create();
            PersonAddRequest personAddRequest4 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse2.CountryID).Create();

            PersonResponse personResponse1 = await this._personService.AddPerson(personAddRequest1);
            PersonResponse personResponse2 = await this._personService.AddPerson(personAddRequest2);
            PersonResponse personResponse3 = await this._personService.AddPerson(personAddRequest3);
            PersonResponse personResponse4 = await this._personService.AddPerson(personAddRequest4);

            //Act
            List<PersonResponse> person_from_GetFiltered = await this._personService.GetFilteredPerson("PersonName", "2");
            this._testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person in person_from_GetFiltered)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }
            List<PersonResponse>? AllPerson = await this._personService.GetAllPersons();

            List<PersonResponse>? filtered_from_AddPerson = AllPerson?.Where(person => person.PersonName.Contains("2")).ToList();

            this._testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in filtered_from_AddPerson)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            //Assert.Equal(filtered_from_AddPerson, person_from_GetFiltered);
            filtered_from_AddPerson.Should().BeEqualTo(person_from_GetFiltered);
        }

        #endregion

        #region GetSortedPersons

        //Visszaadja a megfelelő attribútum szerint sorbarendezett Person listát.
        [Fact]
        public async Task GetSortedPersons_BasedOnName()
        {
            //Arrange
            //CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            //{
            //    Name = "Hungary"
            //};
            //CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            //{
            //    Name = "Austria"
            //};
            CountryAddRequest countryAddRequest1 = this._fixture.Create<CountryAddRequest>();
            CountryAddRequest countryAddRequest2 = this._fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await this._countryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await this._countryService.AddCountry(countryAddRequest2);

            //PersonAddRequest personAddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "Reka",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Female,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = countryResponse1.CountryID
            //};
            //PersonAddRequest personAddRequest2 = new PersonAddRequest()
            //{
            //    PersonName = "Erik",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(2000, 09, 22),
            //    Gender = Gender.Male,
            //    Address = "asd 22.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse2.CountryID
            //};
            //PersonAddRequest personAddRequest3 = new PersonAddRequest()
            //{
            //    PersonName = "Niki",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(1996, 09, 17),
            //    Gender = Gender.Female,
            //    Address = "asd 30.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse2.CountryID
            //};
            //PersonAddRequest personAddRequest4 = new PersonAddRequest()
            //{
            //    PersonName = "Monika",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(1971, 12, 05),
            //    Gender = Gender.Male,
            //    Address = "asd 50.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse1.CountryID
            //};
            PersonAddRequest personAddRequest1 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest2 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest3 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse2.CountryID).Create();
            PersonAddRequest personAddRequest4 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse2.CountryID).Create();

            PersonResponse personResponse1 = await this._personService.AddPerson(personAddRequest1);
            PersonResponse personResponse2 = await this._personService.AddPerson(personAddRequest2);
            PersonResponse personResponse3 = await this._personService.AddPerson(personAddRequest3);
            PersonResponse personResponse4 = await this._personService.AddPerson(personAddRequest4);

            //Act
            this._testOutputHelper.WriteLine("Expected:");
            List<PersonResponse> persons = new List<PersonResponse>()
            {
                personResponse1, personResponse2, personResponse3, personResponse4
            }.OrderBy(person => person.PersonName).ToList();

            foreach (PersonResponse person in persons)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            this._testOutputHelper.WriteLine("Actual:");
            List<PersonResponse> sortedPersons = await this._personService.GetSortedPersons(persons, nameof(PersonResponse.PersonName), SortingOptions.ASC);
            foreach (PersonResponse person in sortedPersons)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            //for (int i = 0; i < persons.Count; i++)
            //{
            //    Assert.Equal(persons[i], sortedPersons[i]);
            //}
            persons.Should().BeEqualTo(sortedPersons);
        }

        [Fact]
        public async Task GetSortedPersons_BasedOnId()
        {
            //Arrange
            //CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            //{
            //    Name = "Hungary"
            //};
            //CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            //{
            //    Name = "Austria"
            //};
            CountryAddRequest countryAddRequest1 = this._fixture.Create<CountryAddRequest>();
            CountryAddRequest countryAddRequest2 = this._fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await this._countryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = await this._countryService.AddCountry(countryAddRequest2);

            //PersonAddRequest personAddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "Reka",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Female,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = countryResponse1.CountryID
            //};
            //PersonAddRequest personAddRequest2 = new PersonAddRequest()
            //{
            //    PersonName = "Erik",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(2000, 09, 22),
            //    Gender = Gender.Male,
            //    Address = "asd 22.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse2.CountryID
            //};
            //PersonAddRequest personAddRequest3 = new PersonAddRequest()
            //{
            //    PersonName = "Niki",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(1996, 09, 17),
            //    Gender = Gender.Female,
            //    Address = "asd 30.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse2.CountryID
            //};
            //PersonAddRequest personAddRequest4 = new PersonAddRequest()
            //{
            //    PersonName = "Monika",
            //    Email = "asd2@gmail.com",
            //    DateOfBirth = new DateTime(1971, 12, 05),
            //    Gender = Gender.Male,
            //    Address = "asd 50.",
            //    ReceiveNewsLetter = false,
            //    CountryId = countryResponse1.CountryID
            //};
            PersonAddRequest personAddRequest1 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest2 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse1.CountryID).Create();
            PersonAddRequest personAddRequest3 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse2.CountryID).Create();
            PersonAddRequest personAddRequest4 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse2.CountryID).Create();

            PersonResponse personResponse1 = await this._personService.AddPerson(personAddRequest1);
            PersonResponse personResponse2 = await this._personService.AddPerson(personAddRequest2);
            PersonResponse personResponse3 = await this._personService.AddPerson(personAddRequest3);
            PersonResponse personResponse4 = await this._personService.AddPerson(personAddRequest4);

            //Act
            this._testOutputHelper.WriteLine("Expected:");
            List<PersonResponse> persons = new List<PersonResponse>()
            {
                personResponse1, personResponse2, personResponse3, personResponse4
            }.OrderBy(person => person.PersonId).ToList();

            foreach (PersonResponse person in persons)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            this._testOutputHelper.WriteLine("Actual:");
            List<PersonResponse> sortedPersons = await this._personService.GetSortedPersons(persons, nameof(PersonResponse.PersonId), SortingOptions.ASC);
            foreach (PersonResponse person in sortedPersons)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            //for (int i = 0; i < persons.Count; i++)
            //{
            //    Assert.Equal(persons[i], sortedPersons[i]);
            //}
            sortedPersons.Should().BeInAscendingOrder(prop => prop.PersonId);
        }
        #endregion

        #region PersonUpdateRequest

        //Ha megfelelő objektumot adunk át frissítésre, akkor visszaadja a megfelelő értékekkel.
        [Fact]
        public async Task PeresonUpdateRequest_ProperPersonAsync()
        {
            //Arrange
            //CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            //{
            //    Name = "Hungary"
            //};
            CountryAddRequest countryAddRequest1 = this._fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse1 = await this._countryService.AddCountry(countryAddRequest1);

            //PersonAddRequest personAddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "Reka",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Female,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = countryResponse1.CountryID
            //};
            PersonAddRequest personAddRequest1 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, countryResponse1.CountryID).Create();

            //Act
            PersonResponse person_from_AddRequest = await this._personService.AddPerson(personAddRequest1);
            this._testOutputHelper.WriteLine($"Original\n{person_from_AddRequest.ToString()}");

            person_from_AddRequest.PersonName = "Reka22";
            person_from_AddRequest.Address = "asd Road 22.";
            this._testOutputHelper.WriteLine($"Expected\n{person_from_AddRequest.ToString()}");


            PersonResponse person_from_UpdateRequest = await this._personService.UpdatePerson(person_from_AddRequest.ToPersonUpdateRequest());
            this._testOutputHelper.WriteLine($"Actual\n{person_from_UpdateRequest.ToString()}");

            //Assert
            //Assert.Equal(person_from_AddRequest, person_from_UpdateRequest);
            person_from_AddRequest.Should().Be(person_from_UpdateRequest);
        }

        //Ha null-t adunk paraméterül, akkor ArgumentNullException
        [Fact]
        public async Task PeresonUpdateRequest_NullArgument()
        {
            //Assert
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    //Act
            //    await this._personService.UpdatePerson(null);
            //});
            Func<Task> action = async () =>
            {
                await this._personService.UpdatePerson(null);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //Ha rossz Id adatot adunk paraméterül, akkor ArgumentException
        [Fact]
        public async Task PeresonUpdateRequest_WrongPersonId()
        {
            //Arrange
            //PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest()
            //{
            //    PersonId = new Guid()
            //};
            PersonUpdateRequest personUpdateRequest = this._fixture.Build<PersonUpdateRequest>().With(prop => prop.PersonId, new Guid()).Create();

            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    await this._personService.UpdatePerson(personUpdateRequest);
            //});

            Func<Task> action = async () =>
            {
                await this._personService.UpdatePerson(personUpdateRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //Ha a PersonName null, akkor ArgumentException
        [Fact]
        public async Task PeresonUpdateRequest_NullPersonName()
        {
            //Arrange
            //CountryAddRequest countryAddRequest = new CountryAddRequest() { Name = "Hu" };
            CountryAddRequest countryAddRequest = this._fixture.Create<CountryAddRequest>();

            CountryResponse countryResponse = await this._countryService.AddCountry(countryAddRequest);

            //PersonResponse personResponse = await this._personService.AddPerson(new PersonAddRequest()
            //{
            //    PersonName = "Reka",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Female,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = countryResponse.CountryID
            //});
            PersonResponse personResponse = this._fixture.Create<PersonResponse>();

            //PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            //personUpdateRequest.PersonName = string.Empty;
            personResponse.PersonName = null;

            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    await this._personService.UpdatePerson(personResponse.ToPersonUpdateRequest());
            //});
            Func<Task> action = async () =>
            {
                await this._personService.UpdatePerson(personResponse.ToPersonUpdateRequest());
            };

            await action.Should().ThrowAsync<ArgumentException>();

        }

        #endregion

        #region DeletePerson

        //Megfelelő id-t átadva törli az elemet és 1 lesz az eredmény.
        [Fact]
        public async Task DeletePerson_ProperPersonId()
        {
            //Arrange
            //PersonAddRequest personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "Reka",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Male,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = Guid.NewGuid()
            //};
            //PersonAddRequest personAddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "Reka2",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Male,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = Guid.NewGuid()
            //};
            PersonAddRequest personAddRequest1 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, new Guid()).Create();
            PersonAddRequest personAddRequest = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, new Guid()).Create();

            PersonResponse person_from_AddPerson = await this._personService.AddPerson(personAddRequest);
            PersonResponse person_from_AddPerson1 = await this._personService.AddPerson(personAddRequest1);

            //Act
            List<PersonResponse>? persons_from_GetAllOriginal = await this._personService.GetAllPersons();
            this._testOutputHelper.WriteLine("Original");
            foreach (PersonResponse person in persons_from_GetAllOriginal)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }
            bool success = await this._personService.DeletePerson(person_from_AddPerson.PersonId);
            List<PersonResponse>? persons_from_GetAll_aftere_Deleting = await this._personService.GetAllPersons();
            this._testOutputHelper.WriteLine("Modified");
            foreach (PersonResponse person in persons_from_GetAll_aftere_Deleting)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            //Assert.True(success);
            success.Should().BeTrue();
            //Assert.True(persons_from_GetAll_aftere_Deleting?.Count == 1);
            persons_from_GetAll_aftere_Deleting.Should().HaveCount(1);
            //Assert.DoesNotContain(person_from_AddPerson, persons_from_GetAll_aftere_Deleting);
            persons_from_GetAll_aftere_Deleting.Should().NotContain(person_from_AddPerson);
        }

        //Nem megfelelő id-t átadva nem töröl semmit és 0 lesz az eredményt
        //Arrange
        [Fact]
        public async Task DeletePerson_WrongPersonId()
        {
            //PersonAddRequest personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "Reka",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Male,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = Guid.NewGuid()
            //};
            //PersonAddRequest personAddRequest1 = new PersonAddRequest()
            //{
            //    PersonName = "Reka2",
            //    Email = "asd@gmail.com",
            //    DateOfBirth = new DateTime(2005, 05, 18),
            //    Gender = Gender.Male,
            //    Address = "asd 11.",
            //    ReceiveNewsLetter = true,
            //    CountryId = Guid.NewGuid()
            //};
            PersonAddRequest personAddRequest1 = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, new Guid()).Create();
            PersonAddRequest personAddRequest = this._fixture.Build<PersonAddRequest>().With(prop => prop.CountryId, new Guid()).Create();

            PersonResponse person_from_AddPerson = await this._personService.AddPerson(personAddRequest);
            PersonResponse person_from_AddPerson1 = await this._personService.AddPerson(personAddRequest1);

            //Act
            List<PersonResponse>? persons_from_GetAllOriginal = await this._personService.GetAllPersons();
            this._testOutputHelper.WriteLine("Original");
            foreach (PersonResponse person in persons_from_GetAllOriginal)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            bool success = await this._personService.DeletePerson(new Guid());
            List<PersonResponse>? persons_from_GetAll_aftere_Deleting = await this._personService.GetAllPersons();
            this._testOutputHelper.WriteLine("Modified");
            foreach (PersonResponse person in persons_from_GetAll_aftere_Deleting)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            //Assert.True(persons_from_GetAll_aftere_Deleting?.Count == 2);
            persons_from_GetAll_aftere_Deleting.Should().HaveCount(2);
            //Assert.False(success);
            success.Should().BeFalse();
        }

        #endregion
    }
}