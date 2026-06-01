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
