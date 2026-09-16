using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Application.Interfaces;
using Bilheteria.API.Application.Mappers;
using Bilheteria.API.Domain.Interfaces;

namespace Bilheteria.API.Application.UseCases
{
    public class ProdutoUseCase : IProdutoUseCase
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoUseCase(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<PagedResultDto<ProdutoResponseDto>> ObterTodosAsync(int deslocamento, int registroRetornado)
        {
            var (itens, total) = await _produtoRepository.ObterTodosAsync(deslocamento, registroRetornado);

            return new PagedResultDto<ProdutoResponseDto>
            {
                Itens = itens.Select(x => x.ToResponseDto()),
                TotalRegistros = total,
                Deslocamento = deslocamento,
                RegistroRetornado = registroRetornado
            };
        }

        public async Task<ProdutoResponseDto?> ObterPorIdAsync(int id)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);

            return produto?.ToResponseDto();
        }

        public async Task<ProdutoResponseDto> AdicionarAsync(ProdutoRequestDto dto)
        {
            var produto = await _produtoRepository.AdicionarAsync(dto.ToProdutoEntity());

            return produto.ToResponseDto();
        }

        public async Task<ProdutoResponseDto?> EditarAsync(int id, ProdutoRequestDto dto)
        {
            var produto = await _produtoRepository.EditarAsync(id, dto.ToProdutoEntity());

            return produto?.ToResponseDto();
        }

        public async Task<ProdutoResponseDto?> DeletarAsync(int id)
        {
            var produto = await _produtoRepository.DeletarAsync(id);

            return produto?.ToResponseDto();
        }
    }
}
