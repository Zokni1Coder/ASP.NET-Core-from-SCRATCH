using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelValidationExample.Models
{
    public class Person
    {
        [Required]
        [DisplayName("Name")] //Így a Megjelenő hibaüzenetben nem a "field PersonName" fog megjelenni, hanem a "field Name". 
        public string? PersonName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [MinLength(8, ErrorMessage = "The password must be at least 8 characters long.")] //Saját Error üzenet.
        public string? Password { get; set; }
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        [Range(0, int.MaxValue)]
        public double? Price { get; set; }
        public override string ToString()
        {
            return $"Person object - Name: {PersonName}, Email: {Email}, Phone: {Phone}, Password: {Password}, ConfirmPassword: {ConfirmPassword}, Price: {Price}";
        }
    }
}
