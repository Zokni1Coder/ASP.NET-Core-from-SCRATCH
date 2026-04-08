using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.DTOs
{
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? Name { get; set; }
    }


    // Az ilyen extension osztályok segítségével úgy adhatunk hozzá extra funkcionalitást
    // az Entity osztályhoz (pl. DTO-vá alakítás), hogy nem módosítjuk magát az Entity-t.
    // Ez azért fontos, mert az Entity nem függhet magasabb szintű rétegektől (pl. DTO-k),
    // így elkerülhető a rétegek közötti erős csatolás.

    //FONTOS: Az entity a domain rétek része, míg a DTO az applikáció/api réteg része.
    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryID = country.Guid,
                Name = country.Name,
            };
        }
    }
}
