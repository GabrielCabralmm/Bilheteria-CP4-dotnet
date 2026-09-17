using System.Net;
using System.Net.Http.Json;
using Bilheteria.API.Application.Dtos;

namespace Bilheteria.Tests.Integration
{
    public class FilmeControllerTests : IDisposable
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public FilmeControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Fact]
        [Trait("Integration", "Filme")]
        public async Task Get_DeveRetornarNoContent_QuandoNaoHaFilmes()
        {
            var resposta = await _client.GetAsync("/api/filme");

            Assert.Equal(HttpStatusCode.NoContent, resposta.StatusCode);
        }

        [Fact]
        [Trait("Integration", "Filme")]
        public async Task Post_DeveCriarFilme_ERetornar201()
        {
            var novoFilme = new FilmeRequestDto
            {
                Titulo = "Duna: Parte Dois",
                Genero = "Ficcao Cientifica",
                DuracaoMinutos = 166,
                ClassificacaoIndicativa = "14 anos",
                EmCartaz = true
            };

            var resposta = await _client.PostAsJsonAsync("/api/filme", novoFilme);

            Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);

            var filmeCriado = await resposta.Content.ReadFromJsonAsync<FilmeResponseDto>();

            Assert.NotNull(filmeCriado);
            Assert.Equal("Duna: Parte Dois", filmeCriado!.Titulo);
            Assert.True(filmeCriado.Id > 0);
        }

        [Fact]
        [Trait("Integration", "Filme")]
        public async Task GetPorId_DeveRetornar404_QuandoFilmeNaoExiste()
        {
            var resposta = await _client.GetAsync("/api/filme/999999");

            Assert.Equal(HttpStatusCode.NotFound, resposta.StatusCode);
        }
    }
}