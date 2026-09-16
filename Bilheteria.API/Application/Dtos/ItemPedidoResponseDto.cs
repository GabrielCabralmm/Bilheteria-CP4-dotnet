namespace Bilheteria.API.Application.Dtos
{
    public class ItemPedidoResponseDto
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string? NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
