using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract
{
    public interface IFinnhubService
    {
        /// <summary>
        /// Lekérjük a cégek értékét.
        /// </summary>
        /// <returns>Érték adatokat ad vissza a cégről.</returns>
        public Task<Dictionary<string, object>?> GetQuote();
        /// <summary>
        /// Lekérjük a cégek profilját.
        /// </summary>
        /// <returns>Profil adatokat ad vissza a cégről.</returns>
        public Task<Dictionary<string, object>?> GetProfile();
    }
}
