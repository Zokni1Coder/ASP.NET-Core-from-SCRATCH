using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ModelValidationExample.CustomValidators
{
    public class DateRangeValidationAttribute : ValidationAttribute
    {
        public string OtherPropertyName { get; set; }
        public DateRangeValidationAttribute(string otherPropertyName)
        {
            this.OtherPropertyName = otherPropertyName;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                // Ezzel a sorral a model típusából lekérjük a másik property metadata-ját (PropertyInfo), amit később a konkrét példányból (ObjectInstance) tudunk értékként kinyerni.

                //validationContext.ObjectType → a model típusa (pl. Person)

                //.GetProperty(OtherPropertyName) → a string név alapján visszaadja a PropertyInfo-t, ami leírja a property-t, de ebben nem szerepel az értéke, mert nem egy konkrét egyedet ad vissza, csak egy sablont.Később ezt a PropertyInfo-t használjuk, hogy a konkrét objektum példányból kinyerjük az értéket.
                PropertyInfo? otherProperty = validationContext.ObjectType.GetProperty(OtherPropertyName);

                if (otherProperty != null)
                {
                    //ObjectInstance → maga a konkrét model példány. Ezt hívjuk refelection-nek.
                    DateTime fromDate = (DateTime)otherProperty.GetValue(validationContext.ObjectInstance);

                    DateTime toDate = (DateTime)value;
                    if (toDate < fromDate)
                    {
                        return new ValidationResult($"'From Date' should not be older than {validationContext.DisplayName}.");
                    }
                    else
                        return ValidationResult.Success;
                }                
            }
            return null;
        }
    }
}
