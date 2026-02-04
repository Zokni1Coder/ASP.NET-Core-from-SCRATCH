namespace MathApp
{
    /// <summary>
    /// Kivétel osztály. Akkor hívódik meg, amikor az osztó nulla. 
    /// Az Outwriting eljárás és contextet kap paraméterül, hogy azt felhasználja a html kiíráshoz.
    /// </summary>
    public class NullDivisor : Exception
    {
        public NullDivisor()
        {
        }
        public void Outwriting(HttpContext context)
        {
            context.Response.WriteAsync("You cant divide with 0!");
        }
    }
}
