using ServiceContracts;

namespace Services
{
    //Amikor egy Service-szel működő programot hozol létre, akkor mindig a 
    //Service résszel illik kezdeni.
    //Ez a szervice ezt kb úgy kell elképzelni mint mescben mikor tanultuk a 
    //.dll-eket létrehozni és használni. Mivel egy solutionben lesz két projektünk,
    //meg kell adni a Service referencáját a DIExample-nek. Fontos az irány!
    //Referencia megadása: DIExample/Dependencies->Add Project Reference -> 
    //Kipipálod a Services-t és így el fogod tudni érni a metódusokat a másik projektből is.
    //Hogy "child-scope"-okat tudjunk létrehozni oda minden abban használt objektumot el kell hogy tudjuk dobni (dispose) a using végén. Ezért kell implementálni az IDisposable felületet és annak a Dispose() metódusát.
    public class CitiesService : ICitiesService, IDisposable
    {
        private Guid _id { get; set; }
        private List<string> _cities { get; set; }
        public CitiesService()
        {
            _cities = new List<string>()
             {
                 "London",
                 "Paris",
                 "New York",
                 "Tokyo",
                 "Rome"
             };
            _id = Guid.NewGuid();
        }

        List<string> ICitiesService.GetCities()
        {
            return _cities;
        }
        //Itt az implementált metódusa.
        public void Dispose()
        {
           //Hagyd most üresen.
        }

        public Guid GetGuid()
        {
            return _id;
        }
    }
}
