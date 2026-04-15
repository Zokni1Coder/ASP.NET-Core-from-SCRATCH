using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using ServiceContract.Enums;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace ServiceContract.DTOs
{
    /// <summary>
    /// DTO ami a legtöbb Person Service metódus visszatérési típusa lesz.
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }
        public double? Age { get; set; }
        public string? Country { get; set; }

        /// <summary>
        /// Ezzel a metódussal tudjuk ellenőrizni, hogy két Person objektum azonos-e vagy sem.
        /// </summary>
        /// <param name="obj">Ezzel fogjuk az aktuális Person objektumot összehasonlítani</param>
        /// <returns>Ha az obj és az aktuális Person minden mezője azonos, akkor igaz</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is PersonResponse)
            {
                PersonResponse temp = (PersonResponse)obj;
                return (temp.PersonId == this.PersonId && temp.Email == this.Email && temp.PersonName == this.PersonName && temp.DateOfBirth == this.DateOfBirth && temp.CountryId == this.CountryId && temp.Address == this.Address && temp.ReceiveNewsLetter == this.ReceiveNewsLetter && temp.Gender == this.Gender);
            }
            return false;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        //Hogy ki tudjuk szépen íratni az objektumot. 
        public override string ToString()
        {
            return $"Id: {this.PersonId}, Email: {this.Email}, Name: {this.PersonName}, Date of birth: {this.DateOfBirth.ToString()}, CountryId: {this.CountryId}, Country: {this.Country} Address: {this.Address}, ReceivesNewsLetter: {this.ReceiveNewsLetter}";
        }

        /// <summary>
        /// Mivel PersonResponse lesz amit kiválasztunk módosításra, ezért kell egy metódus ami azt átalakítja PersonUpdateRequest-re.
        /// </summary>
        /// <returns>Visszaadja a PersonUpdateRequest-é alakított PersonResponse objektumot.</returns>
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonId = this.PersonId,
                Email = this.Email,
                PersonName = this.PersonName,
                DateOfBirth = this.DateOfBirth,
                CountryId = this.CountryId,
                Address = this.Address,
                ReceiveNewsLetter = this.ReceiveNewsLetter,
                Gender = (Gender)Enum.Parse(typeof(Gender), Gender, true)
            };
        }
    }

    /// <summary>
    /// Kibővítjük a Person objektumot egy PersonResponse-zá alakító függvénnyel.
    /// </summary>
    public static class PersonExtension
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address,
                ReceiveNewsLetter = person.ReceiveNewsLetter,
                Age = (person.DateOfBirth is not null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
            };
        }
    }
}
