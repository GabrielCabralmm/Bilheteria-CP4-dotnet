using System.Net;
using System.Net.Http.Json;
using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Domain.Entities;
using Bilheteria.API.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Bilheteria.Tests.Integration
{
    public class PedidoControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public PedidoControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private async Task<int> SemearSessaoAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            var filme = new FilmeEntity { Titulo = "Filme de Teste", DuracaoMinutos = 100 };
            context.Filme.Add(filme);
            await context.SaveChangesAsync();

            var sessao = new SessaoEntity
            {
                FilmeId = filme.Id,
                Sala = "Sala 1",
                PrecoIngresso = 30m,
                CapacidadeTotal = 50,
                DataHoraSessao = DateTime.UtcNow.AddHours(3)
            };
            context.Sessao.Add(sessao);
            await context.SaveChangesAsync();

            return sessao.Id;
        }

        [Fact]
        [Trait("Integration", "Pedido")]
        public async Task Post_DeveConfirmarCompra_ERetornar201()
        {
            var sessaoId = await SemearSessaoAsync();

            var pedido = new PedidoRequestDto
            {
                SessaoId = sessaoId,
                NomeCliente = "Gabriel Mariano",
                Ingressos = [new IngressoRequestDto { Fileira = "A", NumeroAssento = 1 }]
            };

            var resposta = await _client.PostAsJsonAsync("/api/pedido", pedido);

            Assert.Equal(HttpStatusCode.Created, resposta.StatusCode);

            var pedidoCriado = await resposta.Content.ReadFromJsonAsync<PedidoResponseDto>();

            Assert.NotNull(pedidoCriado);
            Assert.Equal(30m, pedidoCriado!.ValorTotal);
        }

        [Fact]
        [Trait("Integration", "Pedido")]
        public async Task Post_DeveRetornar409_QuandoAssentoJaFoiVendido()
        {
            var sessaoId = await SemearSessaoAsync();

            var pedido = new PedidoRequestDto
            {
                SessaoId = sessaoId,
                NomeCliente = "Primeiro Cliente",
                Ingressos = [new IngressoRequestDto { Fileira = "B", NumeroAssento = 5 }]
            };

            var primeiraResposta = await _client.PostAsJsonAsync("/api/pedido", pedido);
            Assert.Equal(HttpStatusCode.Created, primeiraResposta.StatusCode);

            var segundoPedido = new PedidoRequestDto
            {
                SessaoId = sessaoId,
                NomeCliente = "Segundo Cliente",
                Ingressos = [new IngressoRequestDto { Fileira = "B", NumeroAssento = 5 }]
            };

            var segundaResposta = await _client.PostAsJsonAsync("/api/pedido", segundoPedido);

            Assert.Equal(HttpStatusCode.Conflict, segundaResposta.StatusCode);
        }
    }
}
