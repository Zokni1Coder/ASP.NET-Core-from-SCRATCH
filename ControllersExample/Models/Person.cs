namespace ControllersExample.Models
{

    //Nyiss egy Models mappát a projekten belül és hozd létre benne ezt az objektumot. (Ez már eléggé egy finom átcsatolás az MVC-re is, ami a Model-View-Controller. Most vesszük a Controllereket és most megjelent az első Modellünk is).
    public class Person
    {
        public Guid ID { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int age { get; set; }
    }
}
