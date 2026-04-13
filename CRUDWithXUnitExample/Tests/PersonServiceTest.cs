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
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;

        public PersonServiceTest()
        {
            this._personService = new PersonService();
        }

        #region AddPerson
        /// <summary>
        /// Ha null-t adunk át paraméterül, akkor ArgumentNullException kell hogy legyen
        /// </summary>
        [Fact]
        public void AddPerson_AddNullValue()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act:
                this._personService.AddPerson(null);
            });
        }

        /// <summary>
        /// Ha az PersonAddRequest Name értéke null, akkor ArgumentException kell hogy legyen
        /// </summary>
        [Fact]
        public void AddPerson_PersonAddRequestNameIsNull()
        {
            //Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = null
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act:
                this._personService.AddPerson(personAddRequest);
            });
        }

        /// <summary>
        /// Ha megfelelő Person-t adunk hozzá, akkor bele kell hogy kerüljön a listába és egy PersonResponse objektumot kell hogy visszaadjon az újonnan generált PersonId-val.
        /// </summary>
        [Fact]
        public void AddPerson_ProperPerson()
        {
            //Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "Reka",
                Email = "asd@gmail.com",
                DateOfBirth = new DateTime(2005, 05, 18),
                Gender = Gender.Male,
                Address = "asd 11.",
                ReceiveNewsLetter = true,
                CountryId = Guid.NewGuid()
            };
            //Act
            PersonResponse personResponse_from_Add = this._personService.AddPerson(personAddRequest);
            List<PersonResponse> personsList = this._personService.GetAllPersons();

            //Asserts
            Assert.True(personResponse_from_Add.PersonId != Guid.Empty);
            //Assert.Equal(personResponse_from_Add.ReceiveNewsLetter, personsList[0].ReceiveNewsLetter);
            Assert.Contains(personResponse_from_Add, this._personService.GetAllPersons());
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
        public void GetPersonById_EmptyArgument()
        {
            //Arrange
            Guid guid = Guid.Empty;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                this._personService.GetPersonById(guid);
            });
        }


        //Ha megfelelő guid-t adunk át, akkor megtalálja és visszaadja PersonResponse objektumként
        [Fact]
        public void GetPersonById()
        {
            //Arrange
            PersonAddRequest request = new PersonAddRequest()
            {
                PersonName = "Reka",
                Email = "asd@gmail.com",
                DateOfBirth = new DateTime(2005, 05, 18),
                Gender = Gender.Male,
                Address = "asd 11.",
                ReceiveNewsLetter = true,
                CountryId = Guid.NewGuid()
            };

            //Act:
            PersonResponse person_from_AddPerson = this._personService.AddPerson(request);
            PersonResponse person_from_GetPersonById = this._personService.GetPersonById(person_from_AddPerson.PersonId);

            //Assert
            Assert.Equal(person_from_AddPerson, person_from_GetPersonById);
        }

        //Ha olyan id-t adunk meg, ami nem létezik, akkor a visszatérési objektum null lesz.
        [Fact]
        public void GetPersonByName_DoesntExistingGuidInTheListArgument()
        {
            //Arrange
            PersonAddRequest request = new PersonAddRequest()
            {
                PersonName = "Reka",
                Email = "asd@gmail.com",
                DateOfBirth = new DateTime(2005, 05, 18),
                Gender = Gender.Male,
                Address = "asd 11.",
                ReceiveNewsLetter = true,
                CountryId = Guid.NewGuid()
            };
            //Act:
            PersonResponse person_from_AddPerson = this._personService.AddPerson(request);
            PersonResponse person_from_GetPersonById = this._personService.GetPersonById(person_from_AddPerson.PersonId);

            //Assert
            Assert.Null(person_from_GetPersonById);
        }

        #endregion
    }
}
