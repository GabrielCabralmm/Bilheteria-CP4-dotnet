using Bilheteria.API.Application.Dtos;

namespace Bilheteria.API.Application.Interfaces
{
    public interface IProdutoUseCase
    {
        Task<PagedResultDto<ProdutoResponseDto>> ObterTodosAsync(int deslocamento, int registroRetornado);
        Task<ProdutoResponseDto?> ObterPorIdAsync(int id);
        Task<ProdutoResponseDto> AdicionarAsync(ProdutoRequestDto dto);
        Task<ProdutoResponseDto?> EditarAsync(int id, ProdutoRequestDto dto);
        Task<ProdutoResponseDto?> DeletarAsync(int id);
    }
}
