using Bilheteria.API.Domain.Entities;

namespace Bilheteria.API.Domain.Interfaces
{
    public interface ISessaoRepository
    {
        Task<(IEnumerable<SessaoEntity> Itens, int Total)> ObterTodosAsync(int deslocamento, int registroRetornado);
        Task<SessaoEntity?> ObterPorIdAsync(int id);
        Task<SessaoEntity> AdicionarAsync(SessaoEntity entity);
        Task<SessaoEntity?> EditarAsync(int id, SessaoEntity entity);
        Task<SessaoEntity?> DeletarAsync(int id);
    }
}
