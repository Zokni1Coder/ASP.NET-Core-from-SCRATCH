namespace Entities
{
    /// <summary>
    /// Ez maga a Country objektum, nem DTO, azaz Entity vagy Domain.
    /// </summary>
    public class Country
    {
        public Guid Guid { get; set; }
        public string? Name { get; set; }
    }
}
