using System.ComponentModel.DataAnnotations;

namespace Bilheteria.API.Application.Dtos
{
    public class ItemPedidoRequestDto
    {
        [Required]
        public int ProdutoId { get; set; }

        [Required]
        [Range(1, 999)]
        public int Quantidade { get; set; }
    }
}
