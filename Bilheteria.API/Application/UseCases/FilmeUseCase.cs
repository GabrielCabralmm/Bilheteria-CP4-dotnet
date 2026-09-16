using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Application.Interfaces;
using Bilheteria.API.Application.Mappers;
using Bilheteria.API.Domain.Interfaces;

namespace Bilheteria.API.Application.UseCases
{
    public class FilmeUseCase : IFilmeUseCase
    {
        private readonly IFilmeRepository _filmeRepository;

        public FilmeUseCase(IFilmeRepository filmeRepository)
        {
            _filmeRepository = filmeRepository;
        }

        public async Task<PagedResultDto<FilmeResponseDto>> ObterTodosAsync(int deslocamento, int registroRetornado)
        {
            var (itens, total) = await _filmeRepository.ObterTodosAsync(deslocamento, registroRetornado);

            return new PagedResultDto<FilmeResponseDto>
            {
                Itens = itens.Select(x => x.ToResponseDto()),
                TotalRegistros = total,
                Deslocamento = deslocamento,
                RegistroRetornado = registroRetornado
            };
        }

        public async Task<IEnumerable<FilmeResponseDto>> ObterEmCartazAsync()
        {
            var filmes = await _filmeRepository.ObterEmCartazAsync();

            return filmes.Select(x => x.ToResponseDto());
        }

        public async Task<FilmeResponseDto?> ObterPorIdAsync(int id)
        {
            var filme = await _filmeRepository.ObterPorIdAsync(id);

            return filme?.ToResponseDto();
        }

        public async Task<FilmeResponseDto> AdicionarAsync(FilmeRequestDto dto)
        {
            var filme = await _filmeRepository.AdicionarAsync(dto.ToFilmeEntity());

            return filme.ToResponseDto();
        }

        public async Task<FilmeResponseDto?> EditarAsync(int id, FilmeRequestDto dto)
        {
            var filme = await _filmeRepository.EditarAsync(id, dto.ToFilmeEntity());

            return filme?.ToResponseDto();
        }

        public async Task<FilmeResponseDto?> DeletarAsync(int id)
        {
            var filme = await _filmeRepository.DeletarAsync(id);

            return filme?.ToResponseDto();
        }
    }
}
