using Bilheteria.API.Application.Dtos;

namespace Bilheteria.API.Application.Interfaces
{
    public interface IPedidoUseCase
    {
        Task<PagedResultDto<PedidoResponseDto>> ObterTodosAsync(int deslocamento, int registroRetornado);
        Task<PedidoResponseDto?> ObterPorIdAsync(int id);
        Task<PedidoResponseDto> CriarPedidoAsync(PedidoRequestDto dto);
        Task<PedidoResponseDto?> CancelarAsync(int id);
    }
}
