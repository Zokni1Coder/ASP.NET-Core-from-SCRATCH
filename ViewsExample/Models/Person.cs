namespace ViewsExample.Models
{
    public class Person
    {
        public string? Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public List<string> FavoriteCities { get; set; }
    }

    //Amikor egy változó sűrűbben van használva és mindig fix előre meghatározott értékeket vehet fel, akkor célszerű enumokat használni.
    public enum Gender
    {
        male, female
    }
}
