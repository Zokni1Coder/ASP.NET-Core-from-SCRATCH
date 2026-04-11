using Entities;
using ServiceContract.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceContract.DTOs
{
    /// <summary>
    /// DTO a Person beillesztésére
    /// </summary>
    public class PersonAddRequest
    {
        //Ugyanazok az Attribútumok vannak, mint amiket korábban a Middleware-eknél tanulunk.
        [Required(ErrorMessage = "Person Name can't be empty. ")]
        public string? PersonName { get; set; }
        [EmailAddress(ErrorMessage = "Email address should be valid email.")]                                 
        [Required(ErrorMessage = "Email can't be empty.")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }

        /// <summary>
        /// Átalakítja a RequestAddPerson objektumot Person objektummá
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryId = CountryId,
                Address = Address,
                ReceiveNewsLetter = ReceiveNewsLetter
            };
        }
    }
}
