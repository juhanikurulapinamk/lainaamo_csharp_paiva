
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CreateItemReq
    {
        [Required]
        [MinLength(2), MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        // [Required] ei toimi int-tyypille, koska sillä on aina arvo (oletuksena 0).
        // Range varmistaa, että kategorian id on annettu.
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
    }
}