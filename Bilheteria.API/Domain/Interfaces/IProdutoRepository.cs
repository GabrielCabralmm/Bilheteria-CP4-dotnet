using Bilheteria.API.Domain.Entities;

namespace Bilheteria.API.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<(IEnumerable<ProdutoEntity> Itens, int Total)> ObterTodosAsync(int deslocamento, int registroRetornado);
        Task<ProdutoEntity?> ObterPorIdAsync(int id);
        Task<ProdutoEntity> AdicionarAsync(ProdutoEntity entity);
        Task<ProdutoEntity?> EditarAsync(int id, ProdutoEntity entity);
        Task<ProdutoEntity?> DeletarAsync(int id);
    }
}
