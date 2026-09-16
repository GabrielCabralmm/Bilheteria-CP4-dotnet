using System.ComponentModel.DataAnnotations;

namespace Bilheteria.API.Application.Dtos
{
    public class FilmeRequestDto
    {
        [Required]
        [StringLength(150, MinimumLength = 1)]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Genero { get; set; }

        [Range(1, 999)]
        public int DuracaoMinutos { get; set; }

        [StringLength(10)]
        public string? ClassificacaoIndicativa { get; set; }

        [StringLength(2000)]
        public string? Sinopse { get; set; }

        public bool EmCartaz { get; set; }
    }
}
