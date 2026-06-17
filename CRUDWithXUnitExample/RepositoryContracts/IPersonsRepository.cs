using Entities;
using ServiceContract.DTOs;
using ServiceContract.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    //Ide most beleírjuk azokat a függvényeket amik a Service-kben vannak. 
    public interface IPersonsRepository
    {
        Task<Person> AddPerson(Person person);
        Task<List<Person>> GetAllPersons();
        Task<Person> GetPersonById(Guid id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate">Itt egy Lambda-expressiont várunk, amit alkalmazni fogunk az elemek szűrésére. Tehát nem konkrét adatokat kapunk, hanem a szűrőt magát.
        /// 
        /// A Func<Person, bool>: bemeneti adat egy Person, a kimeneti adat egy bool. Átadunk egy perszont és visszaadja hogy az megfelel-e (bool). 
        /// </param>
        /// <returns></returns>
        Task<List<Person>> GetFilteredPerson(Expression<Func<Person, bool>> predicate);
        Task<Person> UpdatePerson(Person requestPerson);
        Task<bool> DeletePerson(Guid? personId);
    }
}
