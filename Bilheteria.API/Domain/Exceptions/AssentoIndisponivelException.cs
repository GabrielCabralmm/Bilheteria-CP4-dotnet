namespace Bilheteria.API.Domain.Exceptions
{
    public class AssentoIndisponivelException : Exception
    {
        public AssentoIndisponivelException(string fileira, int numeroAssento)
            : base($"O assento {fileira}{numeroAssento} ja foi vendido para esta sessao.")
        {
        }
    }
}
