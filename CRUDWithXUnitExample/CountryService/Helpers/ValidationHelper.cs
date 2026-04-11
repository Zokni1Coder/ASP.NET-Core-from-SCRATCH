using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        /// <summary>
        /// Újrahasznosítható validációs eljárás
        /// </summary>
        /// <param name="objectAddRequest">A validálni kívánt objektum</param>
        /// <exception cref="ArgumentException">Ha hibá(ka)t talál, akkor bedobja az elsőt</exception>
         
        //Static lesz, mert így nem fog kelleni létrehozni a Service-ben ha alkalmazzuk.
        internal static void PersonServiceValidations(object? objectAddRequest)
        {
            //Ez a validációss környezet ami tartalmazza az objektumot, de tartalmazhat extra infókat is pl.: service-ek (DI)
            ValidationContext validationContext = new ValidationContext(objectAddRequest);

            //Ide kerülnek bele az esetleges hibák a validáció során
            List<ValidationResult> results = new List<ValidationResult>();

            //Validáljuk a konkrét objektumot. True/False paraméter azt szolgálja, hogy csak a [Required] propertyket vizsgálja (False) vagy mindent(True).
            bool IsValid = Validator.TryValidateObject(objectAddRequest, validationContext, results, true);

            if (!IsValid)
            {
                throw new ArgumentException(results.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
