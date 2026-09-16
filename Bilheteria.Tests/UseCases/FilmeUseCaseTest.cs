using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Application.UseCases;
using Bilheteria.API.Domain.Entities;
using Bilheteria.API.Domain.Interfaces;
using Moq;

namespace Bilheteria.Tests.UseCases
{
    public class FilmeUseCaseTest
    {
        private readonly Mock<IFilmeRepository> _filmeRepository;
        private readonly FilmeUseCase _filmeUseCase;

        public FilmeUseCaseTest()
        {
            _filmeRepository = new Mock<IFilmeRepository>();
            _filmeUseCase = new FilmeUseCase(_filmeRepository.Object);
        }

        [Fact]
        [Trait("UseCase", "Filme")]
        public async Task ObterPorIdAsync_DeveRetornarFilme_QuandoExiste()
        {
            var filme = new FilmeEntity { Id = 1, Titulo = "Duna: Parte Dois", DuracaoMinutos = 166 };

            _filmeRepository.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(filme);

            var resultado = await _filmeUseCase.ObterPorIdAsync(1);

            Assert.NotNull(resultado);
            Assert.Equal("Duna: Parte Dois", resultado!.Titulo);
        }

        [Fact]
        [Trait("UseCase", "Filme")]
        public async Task ObterPorIdAsync_DeveRetornarNulo_QuandoNaoExiste()
        {
            _filmeRepository.Setup(r => r.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((FilmeEntity?)null);

            var resultado = await _filmeUseCase.ObterPorIdAsync(99);

            Assert.Null(resultado);
        }

        [Fact]
        [Trait("UseCase", "Filme")]
        public async Task ObterTodosAsync_DeveRetornarPaginaComTotal()
        {
            var filmes = new List<FilmeEntity>
            {
                new() { Id = 1, Titulo = "Filme A" },
                new() { Id = 2, Titulo = "Filme B" }
            };

            _filmeRepository
                .Setup(r => r.ObterTodosAsync(0, 10))
                .ReturnsAsync((filmes, 2));

            var resultado = await _filmeUseCase.ObterTodosAsync(0, 10);

            Assert.Equal(2, resultado.TotalRegistros);
            Assert.Equal(2, resultado.Itens.Count());
        }

        [Fact]
        [Trait("UseCase", "Filme")]
        public async Task AdicionarAsync_DeveMapearDtoParaEntidade()
        {
            var dto = new FilmeRequestDto { Titulo = "Novo Filme", DuracaoMinutos = 120, EmCartaz = true };

            _filmeRepository
                .Setup(r => r.AdicionarAsync(It.IsAny<FilmeEntity>()))
                .ReturnsAsync((FilmeEntity e) => { e.Id = 10; return e; });

            var resultado = await _filmeUseCase.AdicionarAsync(dto);

            Assert.Equal(10, resultado.Id);
            Assert.Equal("Novo Filme", resultado.Titulo);
            Assert.True(resultado.EmCartaz);
        }
    }
}
