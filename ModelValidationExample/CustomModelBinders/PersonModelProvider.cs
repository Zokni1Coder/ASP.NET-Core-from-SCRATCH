using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using ModelValidationExample.Models;

namespace ModelValidationExample.CustomModelBinders
{
    //Jelöld meg és implementáld az IModelBinderProvider felületet.
    public class PersonModelProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            //Mint máshol is, itt is a context-ből tudod kinyerni az alap adatokat. Azokból a framework kikövtkeztett, hogy Person típusú objektumról van szó vagy sem.
            if (context.Metadata.ModelType == typeof(Person))
            {
                //Ha Person-ról van szó, akkor visszaad egy ModelBinder típust, amit később a keretrendszer fog majd létrehozni. 
                return new BinderTypeModelBinder(typeof(PersonModelBinder));
            }
            return null;
        }
    }
}
