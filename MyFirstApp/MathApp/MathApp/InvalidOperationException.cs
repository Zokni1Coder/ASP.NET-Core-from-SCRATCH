namespace MathApp
{
    /// <summary>
    /// Ez a kivételosztály akkor jön elő, amikor nem megfelelő érték szerepel a művelet QueryStringnél és nem lehet parse-olni enumra.
    /// Az Outwriting eljárás és contextet kap paraméterül, hogy azt felhasználja a html kiíráshoz. Mivel a feladat úgy kívánja, 400 lesz a Státusz kód. 
    /// </summary>
    public class InvalidOperationException : Exception
    {
        public InvalidOperationException() { }

        public void Outwriting(HttpContext context)
        {
            context.Response.StatusCode = 400;
            context.Response.WriteAsync("Invalid input for operation!");
        }
    }
}
