using Entities;
using ServiceContract;
using ServiceContract.DTOs;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    }
}
