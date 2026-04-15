using Entities;
using ServiceContract.DTOs;
using ServiceContract.Enums;

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

        /// <summary>
        /// Sorbarendezi a Persons listát a megadott attribútum szerint.
        /// </summary>
        /// <param name="persons">Ezt fogja rendezni</param>
        /// <param name="sortBy">Ez szerint fogjuk sorbarendezni a listát</param>
        /// <param name="sortingoption">Megadja hogy csökkenő vagy növekvő sorrendbe rendezze.</param>
        /// <returns>Visszaadja a rendezet listát.</returns>
        public List<PersonResponse> GetSortedPersons(List<PersonResponse> persons, string sortBy, SortingOptions sortingoption);

        /// <summary>
        /// A paraméterül kapott objektumot módosítja és elmenti.
        /// </summary>
        /// <param name="requestPerson">A módosítani kívánt objektum.</param>
        /// <returns>Visszaadja PersonResponse-á alakítva a módosított objektumot.</returns>
        public PersonResponse UpdatePerson(PersonUpdateRequest? requestPerson);

        /// <summary>
        /// A megadott paraméter alapján törlünk egy Person objektumot
        /// </summary>
        /// <param name="personId">Ez a paraméter alalpján fogjuk megtalálni a törölni kívánt Person ojektumot.</param>
        /// <returns>Bool értéket ad vissza a törlés sikerességétől függően.</returns>
        public bool DeletePerson(Guid? personId);
    }
}
