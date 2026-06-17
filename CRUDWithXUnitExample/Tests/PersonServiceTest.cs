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
using Moq;
using RepositoryContracts;
using System.Linq.Expressions;

namespace Tests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Mock<IPersonsRepository> _personsRepositoryMock;
        private readonly IPersonsRepository _personsRepository;
        private readonly IFixture _fixture;

        //Adjuk hozzá a DI-t alkalmazva az ITestOutputHelper Interface-t.
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            //ApplicationDbContext personsDbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            //this._countryService = new CountryService(personsDbContext);
            //this._personService = new PersonService(_countryService, personsDbContext);
            this._personsRepositoryMock = new Mock<IPersonsRepository>();
            this._personsRepository = this._personsRepositoryMock.Object;

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
            _personService = new PersonService(this._countryService, this._personsRepository);
        }

        #region AddPerson
        /// <summary>
        /// Ha null-t adunk át paraméterül, akkor ArgumentNullException kell hogy legyen
        /// </summary>
        [Fact]
        public async Task AddPerson_AddNullValue_ToBeArgumentNullException()
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
        public async void AddPerson_PersonAddRequestNameIsNull_ShouldBeArgumentException()
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
        public async Task AddPerson_ProperPerson_ShouldBeSuccesful()
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

            Person person = personAddRequest.ToPerson();
            PersonResponse expected_response = person.ToPersonResponse();


            this._personsRepositoryMock.Setup(prop => prop.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);



            //Act
            PersonResponse personResponse_from_Add = await this._personService.AddPerson(personAddRequest);

            expected_response.PersonId = personResponse_from_Add.PersonId;
            //List<PersonResponse>? personsList = await this._personService.GetAllPersons();

            //Asserts
            //Assert.True(personResponse_from_Add.PersonId != Guid.Empty);
            personResponse_from_Add.PersonId.Should().NotBe(Guid.Empty);

            personResponse_from_Add.Should().Be(expected_response);

            //Assert.Equal(personResponse_from_Add.ReceiveNewsLetter, personsList[0].ReceiveNewsLetter);
            //Assert.Contains(personResponse_from_Add, await this._personService.GetAllPersons());
            //personsList.Should().Contain(personResponse_from_Add);
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
        public async Task GetPersonById_NullParam_ShouldBeEmptyArgument()
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
            Person person = this._fixture.Build<Person>().With(prop => prop.CountryID, new Guid()).Without(prop => prop.Country).Create();

            PersonResponse personResponse_expected = person.ToPersonResponse();

            //Act:            
            this._personsRepositoryMock.Setup(method => method.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(person);

            PersonResponse? person_from_GetPersonById = await this._personService.GetPersonById(person.PersonID);

            //Assert
            person_from_GetPersonById.Should().Be(personResponse_expected);
        }

        //Ha olyan id-t adunk meg, ami nem létezik, akkor a visszatérési objektum null lesz.
        [Fact]
        public async Task GetPersonById_DoesntExistingGuidInTheListArgument()
        {
            PersonAddRequest request = this._fixture.Build<PersonAddRequest>().With(prop => prop.Email, "asd@gamil.com").Create();
            Person person = request.ToPerson();

            //Act:
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
            List<Person> empty_persons = new List<Person>();
            this._personsRepositoryMock.Setup(method => method.GetAllPersons()).ReturnsAsync(empty_persons);

            //Act
            List<PersonResponse>? persons = new List<PersonResponse>();
            persons = await this._personService.GetAllPersons();

            //Assert
            persons.Should().BeEmpty();
        }

        //Megfelelő elemeket ad vissza a lekérdezés.
        [Fact]
        public async Task GetAllPersons_ProperElements()
        {
            Person person1 = this._fixture.Build<Person>().With(prop => prop.Country, null as Country).Create();
            Person person2 = this._fixture.Build<Person>().With(prop => prop.Country, null as Country).Create();

            List<Person> persons = new List<Person>();
            persons.Add(person1);
            persons.Add(person2);

            List<PersonResponse> expected = persons.Select(person => person.ToPersonResponse()).ToList();

            this._personsRepositoryMock.Setup(method => method.GetAllPersons()).ReturnsAsync(persons);

            List<PersonResponse>? result = await this._personService.GetAllPersons();

            //Asssert
            result.Should().BeEqualTo(expected);
        }

        #endregion

        #region GetFileteredPerson

        //Ha null argumentumokat adunk át keresési értéknek, akkor adja vissza az összeset 
        [Fact]
        public async Task GetFilteredPerson_EmptyArgument()
        {
            //Arrange
            List<Person> persons = new List<Person>();
            Person person1 = this._fixture.Build<Person>().With(prop => prop.Email, "asd@gmail.com").With(prop => prop.Country, null as Country).Create();
            Person person2 = this._fixture.Build<Person>().With(prop => prop.Email, "asd@gmail.com").With(prop => prop.Country, null as Country).Create();
            Person person3 = this._fixture.Build<Person>().With(prop => prop.Email, "asd@gmail.com").With(prop => prop.Country, null as Country).Create();
            persons.Add(person1);
            persons.Add(person2);
            persons.Add(person3);

            List<PersonResponse> expected = persons.Select(person => person.ToPersonResponse()).ToList();

            this._personsRepositoryMock.Setup(method => method.GetAllPersons()).ReturnsAsync(persons);
            this._personsRepositoryMock.Setup(method => method.GetFilteredPerson(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);

            //Act
            List<PersonResponse> result = await this._personService.GetFilteredPerson(nameof(Person.PersonName), "");

            result.Should().BeEqualTo(expected);
        }


        //Ha megfelelő paramétereket adunk át, akkor visszaadja a megfelelő Objektumokat.
        [Fact]
        public async Task GetFilteredPerson_ProperArguments()
        {
            Person person1 = this._fixture.Build<Person>().With(prop => prop.Country, null as Country).With(prop => prop.PersonName, "Reka").Create();
            Person person2 = this._fixture.Build<Person>().With(prop => prop.Country, null as Country).With(prop => prop.PersonName, "Reka1").Create();
            Person person3 = this._fixture.Build<Person>().With(prop => prop.Country, null as Country).With(prop => prop.PersonName, "asd").Create();
            Person person4 = this._fixture.Build<Person>().With(prop => prop.Country, null as Country).With(prop => prop.PersonName, "asd").Create();

            List<Person> allPerson = new List<Person>();
            allPerson.Add(person1);
            allPerson.Add(person2);
            allPerson.Add(person3);
            allPerson.Add(person4);

            List<PersonResponse> expected = new List<PersonResponse>();
            expected.Add(person1.ToPersonResponse());
            expected.Add(person2.ToPersonResponse());

            this._personsRepositoryMock.Setup(method => method.GetAllPersons()).ReturnsAsync(allPerson);

            //Act
            List<PersonResponse> person_from_GetFiltered = await this._personService.GetFilteredPerson(nameof(PersonResponse.PersonName), "Reka");
            this._testOutputHelper.WriteLine("Actual");

            //Assert
            person_from_GetFiltered.Count.Should().Be(expected.Count);
        }

        //Ha megfelelő paramétereket adunk át, akkor visszaadja a megfelelő Objektumokat.
        [Fact]
        public async Task GetFilteredPerson_ProperArgumentsName()
        {

            Person person1 = this._fixture.Build<Person>().With(prop => prop.Country, null as Country).Create();
            Person person2 = this._fixture.Build<Person>().With(prop => prop.Country, null as Country).Create();
            Person person3 = this._fixture.Build<Person>().With(prop => prop.Country, null as Country).Create();
            Person person4 = this._fixture.Build<Person>().With(prop => prop.Country, null as Country).Create();

            List<Person> persons = new List<Person>();
            persons.Add(person1);
            persons.Add(person2);
            persons.Add(person3);
            persons.Add(person4);

            List<PersonResponse> expected = persons.Select(person => person.ToPersonResponse()).ToList();

            //Act
            this._personsRepositoryMock.Setup(method => method.GetAllPersons()).ReturnsAsync(persons);
            //this._personsRepositoryMock.Setup(method => method.GetFilteredPerson(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);

            List<PersonResponse> result = await this._personService.GetFilteredPerson(nameof(Person.PersonName), "sdf");

            result.Count.Should().Be(0);
        }

        #endregion

        #region GetSortedPersons

        //Visszaadja a megfelelő attribútum szerint sorbarendezett Person listát.
        [Fact]
        public async Task GetSortedPersons_BasedOnName()
        {
            PersonResponse PersonResponse1 = this._fixture.Build<PersonResponse>().With(prop => prop.Email, "asd@ghmail.com").Create();
            PersonResponse PersonResponse2 = this._fixture.Build<PersonResponse>().With(prop => prop.Email, "asd@ghmail.com").Create();
            PersonResponse PersonResponse3 = this._fixture.Build<PersonResponse>().With(prop => prop.Email, "asd@ghmail.com").Create();
            PersonResponse PersonResponse4 = this._fixture.Build<PersonResponse>().With(prop => prop.Email, "asd@ghmail.com").Create();
            

            //Act
            this._testOutputHelper.WriteLine("Expected:");
            List<PersonResponse> persons = new List<PersonResponse>()
            {
                PersonResponse1, PersonResponse2, PersonResponse3, PersonResponse4
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
            persons.Should().BeEqualTo(sortedPersons);
        }

        [Fact]
        public async Task GetSortedPersons_BasedOnId()
        {
            PersonAddRequest personAddRequest1 = this._fixture.Build<PersonAddRequest>().With(prop => prop.Email, "asd@gmail.com").Create();
            PersonAddRequest personAddRequest2 = this._fixture.Build<PersonAddRequest>().With(prop => prop.Email, "asd@gmail.com").Create();
            PersonAddRequest personAddRequest3 = this._fixture.Build<PersonAddRequest>().With(prop => prop.Email, "asd@gmail.com").Create();
            PersonAddRequest personAddRequest4 = this._fixture.Build<PersonAddRequest>().With(prop => prop.Email, "asd@gmail.com").Create();

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
            PersonUpdateRequest request = this._fixture.Build<PersonUpdateRequest>().With(prop => prop.Email, "asd@gamil.com").Create();

            Person person = request.ToPerson();
            PersonResponse expected = person.ToPersonResponse();

            this._personsRepositoryMock.Setup(method => method.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);

            //Act
            PersonResponse result = await this._personService.UpdatePerson(request);

            result.Should().Be(expected);
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
            PersonResponse personResponse = this._fixture.Build<PersonResponse>().Without(prop => prop.PersonName).With(prop => prop.Email, "asd@gbmai.com").Create();
            

            //Assert
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
            Guid id = new Guid();

            //Act
            this._personsRepositoryMock.Setup(method => method.DeletePerson(It.IsAny<Guid>())).ReturnsAsync(true);

            bool result = await this._personService.DeletePerson(id);

            //Assert
            result.Should().BeTrue();
        }

        //Nem megfelelő id-t átadva nem töröl semmit és 0 lesz az eredményt
        //Arrange
        [Fact]
        public async Task DeletePerson_WrongPersonId()
        {
            Guid id = new Guid();

            //Act
            this._personsRepositoryMock.Setup(method => method.DeletePerson(It.IsAny<Guid>())).ReturnsAsync(false);

            bool result = await this._personService.DeletePerson(id);

            //Assert
            result.Should().BeFalse();
        }

        #endregion
    }
}