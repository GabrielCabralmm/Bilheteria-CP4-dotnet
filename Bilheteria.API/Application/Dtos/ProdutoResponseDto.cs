namespace Bilheteria.API.Application.Dtos
{
    public class ProdutoResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}
