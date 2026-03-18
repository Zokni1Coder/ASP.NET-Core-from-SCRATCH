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
    public class CitiesService : ICitiesService
    {
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
        }

        List<string> ICitiesService.GetCities()
        {
            return _cities;
        }
    }
}
