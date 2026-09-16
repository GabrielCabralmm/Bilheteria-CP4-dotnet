namespace Bilheteria.API.Application.Dtos
{
    public class PedidoResponseDto
    {
        public int Id { get; set; }
        public int SessaoId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public DateTime DataHoraPedido { get; set; }
        public decimal ValorTotal { get; set; }
        public string StatusPedido { get; set; } = string.Empty;
        public List<IngressoResponseDto> Ingressos { get; set; } = [];
        public List<ItemPedidoResponseDto> Itens { get; set; } = [];
    }
}
