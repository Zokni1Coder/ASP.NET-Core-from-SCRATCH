using Entities;
using ServiceContract.DTOs;

namespace ServiceContract
{
    public interface IPersonService
    {
        /// <summary>
        /// Hozzáadunk a listához egy Person objektumot.
        /// </summary>
        /// <param name="personRequest">PersonAddRequest DTO osztály</param>
        /// <returns>PersonResponse DTO osztály</returns>
        public PersonResponse AddPerson(PersonAddRequest? personRequest);

        /// <summary>
        /// Lekérjük az összes Person objektumot
        /// </summary>
        /// <returns>PersonResponse típusú listát ad vissza</returns>
        public List<PersonResponse>? GetAllPersons();

        /// <summary>
        /// Kikeresi a listából Id awlapján a Person objektumot
        /// </summary>
        /// <param name="id">ez a Guid típusú paraméter alapján keresünk</param>
        /// <returns>Visszaadja a talált Person objektumot PersonResponse-á alakítva</returns>
        public PersonResponse? GetPersonById(Guid? id);

        /// <summary>
        /// Vissza kell hogy adja a paraméterül kapott feltételeeknek megfelelő Person objektumokat
        /// </summary>
        /// <param name="searchBy">A mező(taljdonság pl.: Name, Id) neve, ami alapján keresünk</param>
        /// <param name="searchString">A kiválasztott mezőben ezt az adatot fogjuk keresni</param>
        /// <returns>Egy PersonResponse listát, amiben a megfelelő Person objektumok lesznek</returns>
        public List<PersonResponse> GetFilteredPerson(string searchBy, string? searchString);
    }
}
