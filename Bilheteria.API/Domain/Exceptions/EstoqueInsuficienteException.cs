namespace Bilheteria.API.Domain.Exceptions
{
    public class EstoqueInsuficienteException : Exception
    {
        public EstoqueInsuficienteException(string nomeProduto, int quantidadeEstoque)
            : base($"Estoque insuficiente para o produto '{nomeProduto}'. Restam apenas {quantidadeEstoque} unidades.")
        {
        }
    }
}
