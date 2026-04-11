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
        public List<PersonResponse> GetAllPersons();
    }
}
