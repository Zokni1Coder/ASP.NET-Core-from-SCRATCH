using Entities;
using ServiceContract;
using ServiceContract.DTOs;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person>? _persons;
        private readonly ICountryService _countryService;

        public PersonService()
        {
            this._persons = new List<Person>();
            this._countryService = new CountryService();
        }
        private PersonResponse ToPersonResponseWithCountry(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = this._countryService.GetCountryById(personResponse.CountryId)?.Name;

            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personRequest)
        {
            //Ha null paramétert kap, akkor ArgumentNullException
            //if (personRequest is null)
            //{
            //    throw new ArgumentNullException();
            //}
            //Ha a PersonName property null, akkor ArgumentException
            //if (personRequest.PersonName == null)
            //{
            //    throw new ArgumentException();
            //}

            //Validációs rész:
            ValidationHelper.PersonServiceValidations(personRequest);


            Person tempPerson = personRequest.ToPerson();
            tempPerson.PersonId = Guid.NewGuid();

            this._persons?.Add(tempPerson);

            return ToPersonResponseWithCountry(tempPerson);
        }

        public List<PersonResponse>? GetAllPersons()
        {
            List<PersonResponse>? persons = new List<PersonResponse>();
            foreach (Person person in this._persons)
            {
                persons.Add(ToPersonResponseWithCountry(person));
            }
            return persons;
        }
        public PersonResponse? GetPersonById(Guid? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException();
            }

            Person? person = this._persons.FirstOrDefault(p => p.PersonId == id);

            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }

        public List<PersonResponse> GetFilteredPerson(string searchBy, string? searchString)
        {
            List<PersonResponse>? persons_from_GetAll = GetAllPersons();
            List<PersonResponse>? filtered_persons = new List<PersonResponse>();

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                return persons_from_GetAll;

            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    filtered_persons = persons_from_GetAll.Where(person => person.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(Person.Email):
                    filtered_persons = persons_from_GetAll.Where(person => person.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(Person.DateOfBirth):
                    filtered_persons = persons_from_GetAll.Where(person => person.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(Person.Gender):
                    filtered_persons = persons_from_GetAll.Where(person => person.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(Person.CountryId):
                    filtered_persons = persons_from_GetAll.Where(person => person.CountryId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(Person.Address):
                    filtered_persons = persons_from_GetAll.Where(person => person.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                default:
                    break;
            }
            return filtered_persons;
        }
    }
}
