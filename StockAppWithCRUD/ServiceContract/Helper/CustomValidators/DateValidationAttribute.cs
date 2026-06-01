using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ServiceContract.Helper.CustomValidators
{
    public class DateValidationAttribute : ValidationAttribute
    {
        public DateValidationAttribute()
        {
            ErrorMessage = "The date should be newer than 31.12.1999.";
        }
        public DateValidationAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DateTime? date = (DateTime?)value;
            if (date?.Year < 2000)
            {
                return new ValidationResult(string.Format(ErrorMessage));
            }
            return ValidationResult.Success;
        }
    }
}
