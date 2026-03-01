using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelValidationExample.Models;

namespace ModelValidationExample.CustomModelBinders
{
    //Megjelölöd felületnek az IModelBinder-t és implementálod a metódusát. 
    public class PersonModelBinder : IModelBinder
    {
        //Létre kell hozni egy Person eegyedet, amit a végén tovább adsz és az fog validálásra kerülni.
        Person person = new Person();
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Lekérjük a "FirstName" kulcshoz tartozó értéket. Ha van hozzá beérkező adat, akkor feldolgozzuk.
            if (bindingContext.ValueProvider.GetValue("FirstName").Length > 0)
            {
                //Még a "kurzus" elején szó volt róla, hogy egy Key tartalmazhat több értéket, ezért mi kivesszük a legelsőt, a többivel -ha van-, nem foglalkozunk.
                this.person.PersonName = bindingContext.ValueProvider.GetValue("FirstName").FirstValue;
            }

            if (bindingContext.ValueProvider.GetValue("LastName").Length > 0)
            {
                this.person.PersonName += " " + bindingContext.ValueProvider.GetValue("LastName").FirstValue;
            }

            if (bindingContext.ValueProvider.GetValue("Phone").Length > 0)
            {
                this.person.Phone = bindingContext.ValueProvider.GetValue("Phone").FirstValue;
            }
            if (bindingContext.ValueProvider.GetValue("Email").Length > 0)
            {
                this.person.Email = bindingContext.ValueProvider.GetValue("Email").FirstValue;
            }

            if (bindingContext.ValueProvider.GetValue("Price").Length > 0)
            {
                this.person.Price = Convert.ToDouble(bindingContext.ValueProvider.GetValue("Price").FirstValue);
            }

            if (bindingContext.ValueProvider.GetValue("DateOfBirth").Length > 0)
            {
                this.person.DateOfBirth = Convert.ToDateTime(bindingContext.ValueProvider.GetValue("DateOfBirth").FirstValue);
            }

            if (bindingContext.ValueProvider.GetValue("Tag").Length > 0)
            {
                this.person.Tags = bindingContext.ValueProvider.GetValue("Tag").ToList();
            }

            // A binding eredményét sikeresnek jelöljük, és visszaadjuk a létrehozott Person objektumot.
            bindingContext.Result = ModelBindingResult.Success(person);

            /// Mivel a metódus aszinkron Task-ot vár vissza, egy már befejezett Task-ot adunk vissza.
            return Task.CompletedTask;
        }
    }
}
