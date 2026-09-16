using Bilheteria.API.Domain.Entities;
using Bilheteria.API.Infrastructure.Data;
using Bilheteria.API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bilheteria.Tests.Repositories
{
    public class FilmeRepositoryTest
    {
        private static ApplicationContext CriarContexto(string nomeBanco)
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: nomeBanco)
                .Options;

            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        [Trait("Repository", "Filme")]
        public async Task ObterTodosAsync_DeveRetornarFilmesPaginados()
        {
            using var context = CriarContexto(nameof(ObterTodosAsync_DeveRetornarFilmesPaginados));
            var repository = new FilmeRepository(context);

            context.Filme.AddRange(
                new FilmeEntity { Titulo = "Filme A", DuracaoMinutos = 100 },
                new FilmeEntity { Titulo = "Filme B", DuracaoMinutos = 110 },
                new FilmeEntity { Titulo = "Filme C", DuracaoMinutos = 120 });
            await context.SaveChangesAsync();

            var (itens, total) = await repository.ObterTodosAsync(0, 2);

            Assert.Equal(3, total);
            Assert.Equal(2, itens.Count());
        }

        [Fact]
        [Trait("Repository", "Filme")]
        public async Task ObterEmCartazAsync_DeveRetornarApenasFilmesEmCartaz()
        {
            using var context = CriarContexto(nameof(ObterEmCartazAsync_DeveRetornarApenasFilmesEmCartaz));
            var repository = new FilmeRepository(context);

            context.Filme.AddRange(
                new FilmeEntity { Titulo = "Em cartaz", DuracaoMinutos = 100, EmCartaz = true },
                new FilmeEntity { Titulo = "Fora de cartaz", DuracaoMinutos = 100, EmCartaz = false });
            await context.SaveChangesAsync();

            var resultado = await repository.ObterEmCartazAsync();

            Assert.Single(resultado);
            Assert.Equal("Em cartaz", resultado.First().Titulo);
        }

        [Fact]
        [Trait("Repository", "Filme")]
        public async Task AdicionarAsync_DevePersistirFilme()
        {
            using var context = CriarContexto(nameof(AdicionarAsync_DevePersistirFilme));
            var repository = new FilmeRepository(context);

            var filme = new FilmeEntity { Titulo = "Novo Filme", DuracaoMinutos = 95 };

            var resultado = await repository.AdicionarAsync(filme);

            var filmeNoBanco = await context.Filme.FirstOrDefaultAsync(x => x.Id == resultado.Id);

            Assert.NotNull(filmeNoBanco);
            Assert.Equal("Novo Filme", filmeNoBanco!.Titulo);
        }

        [Fact]
        [Trait("Repository", "Filme")]
        public async Task DeletarAsync_DeveRetornarNulo_QuandoFilmeNaoExiste()
        {
            using var context = CriarContexto(nameof(DeletarAsync_DeveRetornarNulo_QuandoFilmeNaoExiste));
            var repository = new FilmeRepository(context);

            var resultado = await repository.DeletarAsync(999);

            Assert.Null(resultado);
        }
    }
}
