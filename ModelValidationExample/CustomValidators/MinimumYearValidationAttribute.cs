using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace ModelValidationExample.CustomValidations
{
    public class MinimumYearValidationAttribute : ValidationAttribute 
    {
        public int MinimumYear { get; set; } = 2000;
        // Opcionális paraméter nélküli konstruktor.
        // Akkor szükséges, ha az attribútumot paraméter nélkül szeretnénk használni.
        public MinimumYearValidationAttribute(){}
        // Konstruktor a minimum évszám beállításához.
        // Így használható: [MinimumYearValidation(2005)]
        public MinimumYearValidationAttribute(int minimumYear)
        {
           this.MinimumYear = minimumYear; 
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return null;

            DateTime date = (DateTime)value;
            if (date.Year >= this.MinimumYear)
            {
                //return new ValidationResult($"The year should be less than {this.MinimumYear}.");

                //Ha a Person osztályban az attribútumnál megadsz egy error üzenetet, akkor az alábbi két példával tudod megjeleníteni. Máskülönben az első változat használatos.
                //return new ValidationResult($"{ErrorMessage} {this.MinimumYear}.");
                return new ValidationResult(string.Format(ErrorMessage, this.MinimumYear));
            }
            return ValidationResult.Success;
        }
    }
}
