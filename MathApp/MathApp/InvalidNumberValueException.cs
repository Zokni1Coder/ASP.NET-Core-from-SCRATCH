namespace MathApp
{
    /// <summary>
    /// Ezt a kivételt osztályt akkor dobja, amikor nem megfelelő értéket kap az egyik mező, vagy hiányos. A try után catchel adott esetben.
    /// </summary>
    public class InvalidNumberValueException : Exception
    {
        public InvalidNumberValueException() { }
        public void Outwriting()
        {

        }
    }
}
