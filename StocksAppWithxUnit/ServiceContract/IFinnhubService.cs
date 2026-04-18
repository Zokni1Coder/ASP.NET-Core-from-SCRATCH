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
        /// Elküldünk egy requestet a megadott URI-ra használva a User-secretet és a paraméterül kapott értéket.
        /// </summary>
        /// <param name="stockSymbol">Ez a keresett céget fogja jelölni</param>
        /// <returns>Visszaad egy dictionary-t az api válasszal (Cég profil).</returns>
        public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
        /// <summary>
        /// Elküldünk egy requestet a megadott URI-ra használva a User-secretet és a paraméterül kapott értéket.
        /// </summary>
        /// <param name="stockSymbol">Ez a keresett céget fogja jelölni</param>
        /// <returns>Visszaad egy dictionary-t az api válasszal (Cég értéke).</returns>
        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}
