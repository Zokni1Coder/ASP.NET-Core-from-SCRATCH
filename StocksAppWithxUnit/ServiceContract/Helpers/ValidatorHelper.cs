using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.Helpers
{
    public class ValidatorHelper
    {   
        /// <summary>
        /// Itt validáljuk autómatikusan a property-ket az Annotáció segítségével.
        /// </summary>
        /// <param name="RequestObj">Ezt a pparaméterül kappott objektumot validálja</param>
        /// <exception cref="ArgumentException">Sikeres állapotot vagy hibát fog visszaadni.</exception>
        public static void StockServiceValidation(object? RequestObj)
        {
            ValidationContext validationContext = new ValidationContext(RequestObj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool IsValid = Validator.TryValidateObject(RequestObj, validationContext, validationResults, true);

            if (!IsValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}

