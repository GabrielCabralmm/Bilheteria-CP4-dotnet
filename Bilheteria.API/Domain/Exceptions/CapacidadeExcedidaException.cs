namespace Bilheteria.API.Domain.Exceptions
{
    public class CapacidadeExcedidaException : Exception
    {
        public CapacidadeExcedidaException(int capacidadeTotal, int assentosDisponiveis)
            : base($"A sessao tem capacidade para {capacidadeTotal} lugares e restam apenas {assentosDisponiveis} assentos disponiveis.")
        {
        }
    }
}
