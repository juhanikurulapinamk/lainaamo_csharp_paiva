
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    // Tämä tulee sisään rajapintaan
    public class CreateCategoryReq
    {
        [Required(ErrorMessage = "Nimi on pakollinen")]
        [MinLength(2, ErrorMessage = "CATEGORY_NAME_MIN_LENGTH_VALIDATION_ERROR")]
        [MaxLength(50, ErrorMessage = "Nimi saa olla enintään 50 merkkiä")]
        public required string Name { get; set; }
    }
}