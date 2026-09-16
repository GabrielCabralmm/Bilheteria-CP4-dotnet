using Bilheteria.API.Application.Dtos;

namespace Bilheteria.API.Application.Interfaces
{
    public interface ISessaoUseCase
    {
        Task<PagedResultDto<SessaoResponseDto>> ObterTodosAsync(int deslocamento, int registroRetornado);
        Task<SessaoResponseDto?> ObterPorIdAsync(int id);
        Task<SessaoResponseDto> AdicionarAsync(SessaoRequestDto dto);
        Task<SessaoResponseDto?> EditarAsync(int id, SessaoRequestDto dto);
        Task<SessaoResponseDto?> DeletarAsync(int id);
    }
}
