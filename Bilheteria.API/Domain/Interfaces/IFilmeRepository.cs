using Bilheteria.API.Domain.Entities;

namespace Bilheteria.API.Domain.Interfaces
{
    public interface IFilmeRepository
    {
        Task<(IEnumerable<FilmeEntity> Itens, int Total)> ObterTodosAsync(int deslocamento, int registroRetornado);
        Task<IEnumerable<FilmeEntity>> ObterEmCartazAsync();
        Task<FilmeEntity?> ObterPorIdAsync(int id);
        Task<FilmeEntity> AdicionarAsync(FilmeEntity entity);
        Task<FilmeEntity?> EditarAsync(int id, FilmeEntity entity);
        Task<FilmeEntity?> DeletarAsync(int id);
    }
}
