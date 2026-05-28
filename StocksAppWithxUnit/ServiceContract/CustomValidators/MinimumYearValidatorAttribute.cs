using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomValidators
{
    public class MinimumYearValidatorAttribute : ValidationAttribute
    {
        public int minYear { get; set; } = 2000;

        public MinimumYearValidatorAttribute()
        {
            
        }

        public MinimumYearValidatorAttribute(int minYear)
        {
            this.minYear = minYear;
        }
        /// <summary>
        /// Saját Annotáció validálásra. A property értéke nem lehet kisebb a paramétertől.
        /// </summary>
        /// <param name="value">Ez a minimumot jelölő paraméter.Default-ban 2000.</param>
        /// <param name="validationContext">A kontextust jelöli.</param>
        /// <returns>Sikeres állapotot vagy a hibaüzenetet adja vissza.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return null;

            DateTime date = (DateTime)value;

            if (date.Year < minYear)
            {
                return new ValidationResult(errorMessage: "The year can't be older than 01.01.2000.");
            }
            return ValidationResult.Success;
        }
    }
}
