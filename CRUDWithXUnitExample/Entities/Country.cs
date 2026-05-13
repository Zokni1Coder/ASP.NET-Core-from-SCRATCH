using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Ez maga a Country objektum, nem DTO, azaz Entity vagy Domain.
    /// </summary>
    public class Country
    {
        [Key]
        public Guid Guid { get; set; }
        [StringLength(40)]
        public string? Name { get; set; }
    }
}
