namespace ViewComponentsExample.Models
{
    public class CarModel
    {
        public Chassis Chassie { get; set; }
        public string Model { get; set; }
    }

    public enum Chassis
    {
        limousine,
        hothatch,
        pickup,
        suv
    }
}
