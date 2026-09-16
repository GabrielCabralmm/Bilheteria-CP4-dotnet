using Bilheteria.API.Application.Dtos;

namespace Bilheteria.API.Application.Interfaces
{
    public interface IFilmeUseCase
    {
        Task<PagedResultDto<FilmeResponseDto>> ObterTodosAsync(int deslocamento, int registroRetornado);
        Task<IEnumerable<FilmeResponseDto>> ObterEmCartazAsync();
        Task<FilmeResponseDto?> ObterPorIdAsync(int id);
        Task<FilmeResponseDto> AdicionarAsync(FilmeRequestDto dto);
        Task<FilmeResponseDto?> EditarAsync(int id, FilmeRequestDto dto);
        Task<FilmeResponseDto?> DeletarAsync(int id);
    }
}
