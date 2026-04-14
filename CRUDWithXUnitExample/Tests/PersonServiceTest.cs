using ServiceContract;
using ServiceContract.DTOs;
using ServiceContract.Enums;
using Services;
using System;
using Xunit.Abstractions;

namespace Tests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;
        private readonly ITestOutputHelper _testOutputHelper;

        //Adjuk hozzá a DI-t alkalmazva az ITestOutputHelper Interface-t.
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            this._personService = new PersonService();
            this._countryService = new CountryService();
            this._testOutputHelper = testOutputHelper;
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
            Guid? guid = null;
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                this._personService.GetPersonById(guid);
            });
        }

        //Ha megfelelő guid-t adunk át, akkor megtalálja és visszaadja PersonResponse objektumként
        [Fact]
        public void GetPersonById_ProperArgument()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                Name = "Hungary",
            };
            CountryResponse countryResponse_from_AddCountry = this._countryService.AddCountry(countryAddRequest);

            PersonAddRequest request = new PersonAddRequest()
            {
                PersonName = "Reka",
                Email = "asd@gmail.com",
                DateOfBirth = DateTime.Parse("2005-05-18"),
                Gender = Gender.Male,
                Address = "asd 11.",
                ReceiveNewsLetter = true,
                CountryId = countryResponse_from_AddCountry.CountryID
            };

            //Act:
            PersonResponse person_from_AddPerson = this._personService.AddPerson(request);
            PersonResponse? person_from_GetPersonById = this._personService.GetPersonById(person_from_AddPerson.PersonId);

            //Assert
            Assert.Equal(person_from_AddPerson, person_from_GetPersonById);
        }

        //Ha olyan id-t adunk meg, ami nem létezik, akkor a visszatérési objektum null lesz.
        [Fact]
        public void GetPersonById_DoesntExistingGuidInTheListArgument()
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
            PersonResponse? person_from_GetPersonById = new PersonResponse();
            person_from_GetPersonById = this._personService.GetPersonById(Guid.NewGuid());

            //Assert
            Assert.Null(person_from_GetPersonById);
        }

        #endregion

        #region GetAllPerson

        //Az elején üres listát kell hogy visszaadjon.
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse>? persons = new List<PersonResponse>();
            persons = this._personService.GetAllPersons();

            //Assert
            Assert.Empty(persons);
        }

        //Megfelelő elemeket ad vissza a lekérdezés.
        [Fact]
        public void GetAllPersons_ProperElements()
        {
            //Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                Name = "Hungary"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                Name = "Austria"
            };

            CountryResponse countryResponse1 = this._countryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = this._countryService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Reka",
                Email = "asd@gmail.com",
                DateOfBirth = new DateTime(2005, 05, 18),
                Gender = Gender.Female,
                Address = "asd 11.",
                ReceiveNewsLetter = true,
                CountryId = countryResponse1.CountryID
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Reka2",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(2000, 09, 22),
                Gender = Gender.Male,
                Address = "asd 22.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse2.CountryID
            };

            //Act
            PersonResponse personResponse1 = this._personService.AddPerson(personAddRequest1);
            PersonResponse personResponse2 = this._personService.AddPerson(personAddRequest2);

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
            List<PersonResponse>? persons_from_GetAllPersons = this._personService.GetAllPersons();

            foreach (PersonResponse person in persons_from_GetAllPersons)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Asssert
            foreach (PersonResponse person in person_From_AddPerson)
            {
                Assert.Contains(person, persons_from_GetAllPersons);
            }
        }

        #endregion

        #region GetFileteredPerson

        //Ha null argumentumokat adunk át keresési értéknek, akkor adja vissza az összeset 
        [Fact]
        public void GetFilteredPerson_EmptyArgument()
        {
            //Arrange
            //Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                Name = "Hungary"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                Name = "Austria"
            };

            CountryResponse countryResponse1 = this._countryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = this._countryService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Reka",
                Email = "asd@gmail.com",
                DateOfBirth = new DateTime(2005, 05, 18),
                Gender = Gender.Female,
                Address = "asd 11.",
                ReceiveNewsLetter = true,
                CountryId = countryResponse1.CountryID
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Reka2",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(2000, 09, 22),
                Gender = Gender.Male,
                Address = "asd 22.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse2.CountryID
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Reka3",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(1996, 09, 17),
                Gender = Gender.Female,
                Address = "asd 30.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse2.CountryID
            };
            PersonAddRequest personAddRequest4 = new PersonAddRequest()
            {
                PersonName = "Reka4",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(1971, 12, 05),
                Gender = Gender.Male,
                Address = "asd 50.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse1.CountryID
            };
            PersonResponse personResponse1 = this._personService.AddPerson(personAddRequest1);
            PersonResponse personResponse2 = this._personService.AddPerson(personAddRequest2);
            PersonResponse personResponse3 = this._personService.AddPerson(personAddRequest3);
            PersonResponse personResponse4 = this._personService.AddPerson(personAddRequest4);

            //Act
            List<PersonResponse>? person_from_GetAll = this._personService.GetAllPersons();
            this._testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in person_from_GetAll)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            List<PersonResponse>? persons_from_GetFiltered = this._personService.GetFilteredPerson("CountryId", null);
            this._testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in persons_from_GetFiltered)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            Assert.Equal(person_from_GetAll, persons_from_GetFiltered);
        }


        //Ha megfelelő paramétereket adunk át, akkor visszaadja a megfelelő Objektumokat.
        [Fact]
        public void GetFilteredPerson_ProperArguments()
        {
            //Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                Name = "Hungary"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                Name = "Austria"
            };

            CountryResponse countryResponse1 = this._countryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = this._countryService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Reka",
                Email = "asd@gmail.com",
                DateOfBirth = new DateTime(2005, 05, 18),
                Gender = Gender.Female,
                Address = "asd 11.",
                ReceiveNewsLetter = true,
                CountryId = countryResponse1.CountryID
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Reka2",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(2000, 09, 22),
                Gender = Gender.Male,
                Address = "asd 22.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse2.CountryID
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Reka3",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(1996, 09, 17),
                Gender = Gender.Female,
                Address = "asd 30.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse2.CountryID
            };
            PersonAddRequest personAddRequest4 = new PersonAddRequest()
            {
                PersonName = "Reka4",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(1971, 12, 05),
                Gender = Gender.Male,
                Address = "asd 50.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse1.CountryID
            };
            PersonResponse personResponse1 = this._personService.AddPerson(personAddRequest1);
            PersonResponse personResponse2 = this._personService.AddPerson(personAddRequest2);
            PersonResponse personResponse3 = this._personService.AddPerson(personAddRequest3);
            PersonResponse personResponse4 = this._personService.AddPerson(personAddRequest4);

            //Act
            List<PersonResponse> person_from_GetFiltered = this._personService.GetFilteredPerson("CountryId", countryResponse1.CountryID.ToString());
            this._testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person in person_from_GetFiltered)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }
            List<PersonResponse>? filtered_from_AddPerson = this._personService.GetAllPersons().Where(person => person.CountryId == countryResponse1.CountryID).ToList();
            this._testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in filtered_from_AddPerson)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            Assert.Equal(filtered_from_AddPerson, person_from_GetFiltered);
        }

        //Ha megfelelő paramétereket adunk át, akkor visszaadja a megfelelő Objektumokat.
        [Fact]
        public void GetFilteredPerson_ProperArgumentsName()
        {
            //Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                Name = "Hungary"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                Name = "Austria"
            };

            CountryResponse countryResponse1 = this._countryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = this._countryService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Reka",
                Email = "asd@gmail.com",
                DateOfBirth = new DateTime(2005, 05, 18),
                Gender = Gender.Female,
                Address = "asd 11.",
                ReceiveNewsLetter = true,
                CountryId = countryResponse1.CountryID
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Reka2",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(2000, 09, 22),
                Gender = Gender.Male,
                Address = "asd 22.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse2.CountryID
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Reka3",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(1996, 09, 17),
                Gender = Gender.Female,
                Address = "asd 30.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse2.CountryID
            };
            PersonAddRequest personAddRequest4 = new PersonAddRequest()
            {
                PersonName = "Reka4",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(1971, 12, 05),
                Gender = Gender.Male,
                Address = "asd 50.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse1.CountryID
            };
            PersonResponse personResponse1 = this._personService.AddPerson(personAddRequest1);
            PersonResponse personResponse2 = this._personService.AddPerson(personAddRequest2);
            PersonResponse personResponse3 = this._personService.AddPerson(personAddRequest3);
            PersonResponse personResponse4 = this._personService.AddPerson(personAddRequest4);

            //Act
            List<PersonResponse> person_from_GetFiltered = this._personService.GetFilteredPerson("PersonName", "2");
            this._testOutputHelper.WriteLine("Actual");
            foreach (PersonResponse person in person_from_GetFiltered)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }
            List<PersonResponse>? filtered_from_AddPerson = this._personService.GetAllPersons().Where(person => person.PersonName.Contains("2")).ToList();
            this._testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in filtered_from_AddPerson)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            Assert.Equal(filtered_from_AddPerson, person_from_GetFiltered);
        }

        #endregion

        #region GetSortedPersons

        //Visszaadja a megfelelő attribútum szerint sorbarendezett Person listát.
        [Fact]
        public void GetSortedPersons_BasedOnName()
        {
            //Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                Name = "Hungary"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                Name = "Austria"
            };

            CountryResponse countryResponse1 = this._countryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = this._countryService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Reka",
                Email = "asd@gmail.com",
                DateOfBirth = new DateTime(2005, 05, 18),
                Gender = Gender.Female,
                Address = "asd 11.",
                ReceiveNewsLetter = true,
                CountryId = countryResponse1.CountryID
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Erik",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(2000, 09, 22),
                Gender = Gender.Male,
                Address = "asd 22.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse2.CountryID
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Niki",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(1996, 09, 17),
                Gender = Gender.Female,
                Address = "asd 30.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse2.CountryID
            };
            PersonAddRequest personAddRequest4 = new PersonAddRequest()
            {
                PersonName = "Monika",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(1971, 12, 05),
                Gender = Gender.Male,
                Address = "asd 50.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse1.CountryID
            };
            PersonResponse personResponse1 = this._personService.AddPerson(personAddRequest1);
            PersonResponse personResponse2 = this._personService.AddPerson(personAddRequest2);
            PersonResponse personResponse3 = this._personService.AddPerson(personAddRequest3);
            PersonResponse personResponse4 = this._personService.AddPerson(personAddRequest4);

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
            List<PersonResponse> sortedPersons = this._personService.GetSortedPersons(persons, nameof(PersonResponse.PersonName), SortingOptions.ASC);
            foreach (PersonResponse person in sortedPersons)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            for (int i = 0; i < persons.Count; i++)
            {
                Assert.Equal(persons[i], sortedPersons[i]);
            }
        }

        [Fact]
        public void GetSortedPersons_BasedOnId()
        {
            //Arrange
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                Name = "Hungary"
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                Name = "Austria"
            };

            CountryResponse countryResponse1 = this._countryService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = this._countryService.AddCountry(countryAddRequest2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                PersonName = "Reka",
                Email = "asd@gmail.com",
                DateOfBirth = new DateTime(2005, 05, 18),
                Gender = Gender.Female,
                Address = "asd 11.",
                ReceiveNewsLetter = true,
                CountryId = countryResponse1.CountryID
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                PersonName = "Erik",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(2000, 09, 22),
                Gender = Gender.Male,
                Address = "asd 22.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse2.CountryID
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                PersonName = "Niki",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(1996, 09, 17),
                Gender = Gender.Female,
                Address = "asd 30.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse2.CountryID
            };
            PersonAddRequest personAddRequest4 = new PersonAddRequest()
            {
                PersonName = "Monika",
                Email = "asd2@gmail.com",
                DateOfBirth = new DateTime(1971, 12, 05),
                Gender = Gender.Male,
                Address = "asd 50.",
                ReceiveNewsLetter = false,
                CountryId = countryResponse1.CountryID
            };
            PersonResponse personResponse1 = this._personService.AddPerson(personAddRequest1);
            PersonResponse personResponse2 = this._personService.AddPerson(personAddRequest2);
            PersonResponse personResponse3 = this._personService.AddPerson(personAddRequest3);
            PersonResponse personResponse4 = this._personService.AddPerson(personAddRequest4);

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
            List<PersonResponse> sortedPersons = this._personService.GetSortedPersons(persons, nameof(PersonResponse.PersonId), SortingOptions.ASC);
            foreach (PersonResponse person in sortedPersons)
            {
                this._testOutputHelper.WriteLine(person.ToString());
            }

            //Assert
            for (int i = 0; i < persons.Count; i++)
            {
                Assert.Equal(persons[i], sortedPersons[i]);
            }
        }
        #endregion
    }
}
