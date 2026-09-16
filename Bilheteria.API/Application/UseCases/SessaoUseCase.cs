using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Application.Interfaces;
using Bilheteria.API.Application.Mappers;
using Bilheteria.API.Domain.Interfaces;

namespace Bilheteria.API.Application.UseCases
{
    public class SessaoUseCase : ISessaoUseCase
    {
        private readonly ISessaoRepository _sessaoRepository;
        private readonly IPedidoRepository _pedidoRepository;

        public SessaoUseCase(ISessaoRepository sessaoRepository, IPedidoRepository pedidoRepository)
        {
            _sessaoRepository = sessaoRepository;
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PagedResultDto<SessaoResponseDto>> ObterTodosAsync(int deslocamento, int registroRetornado)
        {
            var (itens, total) = await _sessaoRepository.ObterTodosAsync(deslocamento, registroRetornado);

            var itensComDisponibilidade = new List<SessaoResponseDto>();

            foreach (var sessao in itens)
            {
                var vendidos = await _pedidoRepository.ContarIngressosVendidosAsync(sessao.Id);
                itensComDisponibilidade.Add(sessao.ToResponseDto(sessao.CapacidadeTotal - vendidos));
            }

            return new PagedResultDto<SessaoResponseDto>
            {
                Itens = itensComDisponibilidade,
                TotalRegistros = total,
                Deslocamento = deslocamento,
                RegistroRetornado = registroRetornado
            };
        }

        public async Task<SessaoResponseDto?> ObterPorIdAsync(int id)
        {
            var sessao = await _sessaoRepository.ObterPorIdAsync(id);

            if (sessao is null)
                return null;

            var vendidos = await _pedidoRepository.ContarIngressosVendidosAsync(id);

            return sessao.ToResponseDto(sessao.CapacidadeTotal - vendidos);
        }

        public async Task<SessaoResponseDto> AdicionarAsync(SessaoRequestDto dto)
        {
            var sessao = await _sessaoRepository.AdicionarAsync(dto.ToSessaoEntity());

            return sessao.ToResponseDto(sessao.CapacidadeTotal);
        }

        public async Task<SessaoResponseDto?> EditarAsync(int id, SessaoRequestDto dto)
        {
            var sessao = await _sessaoRepository.EditarAsync(id, dto.ToSessaoEntity());

            if (sessao is null)
                return null;

            var vendidos = await _pedidoRepository.ContarIngressosVendidosAsync(id);

            return sessao.ToResponseDto(sessao.CapacidadeTotal - vendidos);
        }

        public async Task<SessaoResponseDto?> DeletarAsync(int id)
        {
            var sessao = await _sessaoRepository.DeletarAsync(id);

            return sessao?.ToResponseDto(0);
        }
    }
}
