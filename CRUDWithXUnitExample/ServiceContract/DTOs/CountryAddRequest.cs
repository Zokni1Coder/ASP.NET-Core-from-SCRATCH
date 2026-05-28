using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.DTOs
{
    public class CountryAddRequest
    {
        //Vedd észre, hogy a request-ben nincs CountryID, csak Name.
        public string? Name { get; set; }

        // Ez a metódus a DTO-t Entity-vé alakítja.
        // Erre azért van szükség, mert a kliens DTO-t küld a szervernek,
        // viszont az adatbázis műveletek Entity objektumokkal történnek.
        // Így a beérkező adatot át kell alakítani a belső modellé.
        public Country ToCountry()
        {
            return new Country { CountryName = Name };
        }
    }
}
