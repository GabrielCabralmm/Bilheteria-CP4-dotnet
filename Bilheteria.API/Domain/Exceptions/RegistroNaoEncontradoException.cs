namespace Bilheteria.API.Domain.Exceptions
{
    public class RegistroNaoEncontradoException : Exception
    {
        public RegistroNaoEncontradoException(string entidade, int id)
            : base($"{entidade} com Id {id} nao foi encontrado.")
        {
        }
    }
}
