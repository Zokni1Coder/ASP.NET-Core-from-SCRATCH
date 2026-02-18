using Microsoft.AspNetCore.Mvc;

namespace ControllersExample.Models
{
    public class Book
    {
        //Ezzel az attribútummal ("[FromRoute]"), azt érjük el, hogy a class kivételt csináljon a BookId propertyvel. Az nem a QueryStringből kapja az értékét, mint a többi, hanem a RoutParam-ból.
        //[FromRoute] 
        public int? BookId { get; set; }
        public string? Author { get; set; }

        public override string ToString()
        {
            return $"Book object - BookID: {BookId}, Author: {Author}";
        }
    }
}
