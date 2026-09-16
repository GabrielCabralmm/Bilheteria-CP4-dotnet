using Bilheteria.API.Domain.Entities;

namespace Bilheteria.API.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<(IEnumerable<PedidoEntity> Itens, int Total)> ObterTodosAsync(int deslocamento, int registroRetornado);
        Task<PedidoEntity?> ObterPorIdAsync(int id);
        Task<bool> AssentoOcupadoAsync(int sessaoId, string fileira, int numeroAssento);
        Task<int> ContarIngressosVendidosAsync(int sessaoId);
        Task<PedidoEntity> AdicionarAsync(PedidoEntity entity);
        Task<PedidoEntity?> AtualizarStatusAsync(int id, string status);
    }
}
