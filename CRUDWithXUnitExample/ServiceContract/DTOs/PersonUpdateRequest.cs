using ServiceContract.Enums;
using System.ComponentModel.DataAnnotations;
using Entities;

namespace ServiceContract.DTOs
{
    /// <summary>
    /// Person objektum firssítésére szolgáló DTO osztály.
    /// </summary>
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "Id can't be empty.")]
        public Guid PersonId { get; set; }
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
        /// Az aktuális PersonUpdateRequest objektumot Person objektummá alakítja.
        /// </summary>
        /// <returns>Visszaadja a Person-ná alakított objektumot.</returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonId = this.PersonId,
                PersonName = this.PersonName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender.ToString(),
                CountryId = this.CountryId,
                Address = this.Address,
                ReceiveNewsLetter = this.ReceiveNewsLetter
            };
        }
    }
}
