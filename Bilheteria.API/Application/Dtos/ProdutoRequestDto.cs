using System.ComponentModel.DataAnnotations;

namespace Bilheteria.API.Application.Dtos
{
    public class ProdutoRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [RegularExpression("Comida|Bebida", ErrorMessage = "Categoria deve ser 'Comida' ou 'Bebida'.")]
        public string Categoria { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 9999.99)]
        public decimal Preco { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int QuantidadeEstoque { get; set; }
    }
}
