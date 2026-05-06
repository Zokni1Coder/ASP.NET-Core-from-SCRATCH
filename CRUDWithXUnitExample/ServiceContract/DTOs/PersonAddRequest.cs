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

        //FONTOS! Ha asp-fort használsz, akkor ezeket a property neveket kell ott megjelölni, mint pl a PersonName és Email.
        //Ha az asp-fort használod, akkor strongly typed kell hogy legyena view, hogy a keretrendszer tudja mihez kötni.

        [Required(ErrorMessage = "Person Name can't be empty. ")]
        [Display (Name = "Person Name")] //Mivel a Create.cshtml label-nél az asp-for-t alkalmaztuk ezért a Display attribútum nélkül a label szövege "PersonName" lenne, így viszont "Person Name".
        public string? PersonName { get; set; }
        [EmailAddress(ErrorMessage = "Email address should be valid email.")]                                 
        [Required(ErrorMessage = "Email can't be empty.")]
        public string? Email { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        [Display(Name = "Country")]
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        [Display(Name = "Receive News Letter")]
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
