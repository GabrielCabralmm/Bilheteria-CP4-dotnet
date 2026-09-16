using System.ComponentModel.DataAnnotations;

namespace Bilheteria.API.Application.Dtos
{
    public class PedidoRequestDto
    {
        [Required]
        public int SessaoId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string NomeCliente { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "E preciso informar pelo menos um ingresso.")]
        public List<IngressoRequestDto> Ingressos { get; set; } = [];

        public List<ItemPedidoRequestDto>? Itens { get; set; } = [];
    }
}
