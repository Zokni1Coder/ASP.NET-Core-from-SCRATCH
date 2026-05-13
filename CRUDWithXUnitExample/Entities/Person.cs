using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entities
{
    /// <summary>
    /// Person domain osztály
    /// </summary>
    public class Person
    {
        [Key] //Ezekkel az attribútumokkal tudjuk a db tábla megszoríásait definiáni.
        public Guid PersonId { get; set; }
        [StringLength(40)] //érdemes megadni a maximumát egy nvarchar-nak, mert ha nem akkor úgy generáódik hogy nvarchar(max), ami nagyon leterhelné a db-t egy idő után. 
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        [StringLength(40)]
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }
    }
}
