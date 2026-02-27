using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ModelValidationExample.CustomValidations;
using ModelValidationExample.CustomValidators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ModelValidationExample.Models
{
    //Itt a megjelölt felületünk.
    public class Person : IValidatableObject
    {
        [Required]
        [DisplayName("Name")] //Így a Megjelenő hibaüzenetben nem a "field PersonName" fog megjelenni, hanem a "field Name".
        [RegularExpression("^[A-Za-z .]*$", ErrorMessage = "{0} should contain only alphabets, space and dot (.).")]
        public string? PersonName { get; set; }

        [EmailAddress]
        [BindNever]
        public string? Email { get; set; }

        [Phone]
        //[ValidateNever] ha az adott property-t nem szeretnénk elleneőrizni.
        public string? Phone { get; set; }

        [MinLength(8, ErrorMessage = "The password should be at least 8 characters long.")] //Saját Error üzenet.
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "{0} and {1} do not match.")]
        [DisplayName("Confirm Password")]
        public string? ConfirmPassword { get; set; }

        [Range(0, 999.99, ErrorMessage = "The Price must be between {1} and {2}.")] //Ebben az esetben az {1} az első paraméter értékre, a {2} a másodikra mutat. a {0}, az minden esetben maga a property neve amit megszorítunk (ha DisplayName annotáció van rajta, akkor azt fogja megejeleníteni). Ezeket a számokat nevezzük Placeholderoknak.
        public double? Price { get; set; }

        //[MinimumYearValidation]
        [MinimumYearValidation(2005, ErrorMessage = "The year should be less than {0}.")]
        public DateTime? DateOfBirth { get; set; }
        [DisplayName("'To Date'")]
        public DateTime? FromDate { get; set; }
        [DateRangeValidation("FromDate")]
        [DisplayName("'To Date'")]
        public DateTime? ToDate { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        //Feladat: Nem lehet egyszerre nul értékű az Age és a DattOfBirth property.
        //1.Lépés: Jelöld meg azIValidaableObject interfacet az osztály felületének.
        //2.Lépés: Implementáld a Validate metódust és abban végezd el a validációt.
        public int? Age { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!DateOfBirth.HasValue && !Age.HasValue)
            {
                //A "yield" kulcsszó arra való, hogy egyszerre több return értékünk legyen. Ezt most azért kötelező használni, mert a visszatérési típus az IEnumerable, ami több értéket is vissza tud adni. 
                yield return new ValidationResult("Date of birth and Age can not be null in the same time.");
            }
        }
        public override string ToString()
        {
            return $"Person object - Name: {PersonName}, Email: {Email}, Phone: {Phone}, Password: {Password}, ConfirmPassword: {ConfirmPassword}, Price: {Price}, Date of Birth: {DateOfBirth}, From Date: {FromDate}, To Date: {ToDate}, Number of Tags: {Tags[0]}.";
        }
    }
}
