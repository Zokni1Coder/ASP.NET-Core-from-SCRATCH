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

            foreach (PersonResponse person in personsList)
            {
                Assert.Contains(personResponse_from_Add, personsList);
            }
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
    }
}
