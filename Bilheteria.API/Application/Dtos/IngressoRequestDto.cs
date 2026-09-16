using System.ComponentModel.DataAnnotations;

namespace Bilheteria.API.Application.Dtos
{
    public class IngressoRequestDto
    {
        [Required]
        [StringLength(2, MinimumLength = 1)]
        public string Fileira { get; set; } = string.Empty;

        [Required]
        [Range(1, 999)]
        public int NumeroAssento { get; set; }
    }
}
