using System.ComponentModel.DataAnnotations;

namespace Bilheteria.API.Application.Dtos
{
    public class SessaoRequestDto
    {
        [Required]
        public int FilmeId { get; set; }

        [Required]
        public DateTime DataHoraSessao { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Sala { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 9999.99)]
        public decimal PrecoIngresso { get; set; }

        [Required]
        [Range(1, 999)]
        public int CapacidadeTotal { get; set; }
    }
}
