using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helper
{
    public class ValidationHelper
    {
        internal static void AddRequestValidator(object? ObjectAddRequest)
        {
            ValidationContext validationContext = new ValidationContext(ObjectAddRequest);

            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool IsValid = Validator.TryValidateObject(ObjectAddRequest, validationContext, validationResults, true);

            if (!IsValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
