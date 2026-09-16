using Bilheteria.API.Domain.Entities;
using Bilheteria.API.Infrastructure.Data;
using Bilheteria.API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bilheteria.Tests.Repositories
{
    public class PedidoRepositoryTest
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
        [Trait("Repository", "Pedido")]
        public async Task AssentoOcupadoAsync_DeveRetornarTrue_QuandoAssentoJaVendido()
        {
            using var context = CriarContexto(nameof(AssentoOcupadoAsync_DeveRetornarTrue_QuandoAssentoJaVendido));
            var repository = new PedidoRepository(context);

            context.Ingresso.Add(new IngressoEntity { SessaoId = 1, Fileira = "D", NumeroAssento = 12, PrecoPago = 30m });
            await context.SaveChangesAsync();

            var ocupadoMesmoAssento = await repository.AssentoOcupadoAsync(1, "D", 12);
            var ocupadoOutroAssento = await repository.AssentoOcupadoAsync(1, "D", 13);

            Assert.True(ocupadoMesmoAssento);
            Assert.False(ocupadoOutroAssento);
        }

        [Fact]
        [Trait("Repository", "Pedido")]
        public async Task ContarIngressosVendidosAsync_DeveContarApenasDaSessaoInformada()
        {
            using var context = CriarContexto(nameof(ContarIngressosVendidosAsync_DeveContarApenasDaSessaoInformada));
            var repository = new PedidoRepository(context);

            context.Ingresso.AddRange(
                new IngressoEntity { SessaoId = 1, Fileira = "A", NumeroAssento = 1, PrecoPago = 30m },
                new IngressoEntity { SessaoId = 1, Fileira = "A", NumeroAssento = 2, PrecoPago = 30m },
                new IngressoEntity { SessaoId = 2, Fileira = "A", NumeroAssento = 1, PrecoPago = 30m });
            await context.SaveChangesAsync();

            var total = await repository.ContarIngressosVendidosAsync(1);

            Assert.Equal(2, total);
        }

        [Fact]
        [Trait("Repository", "Pedido")]
        public async Task AdicionarAsync_DevePersistirPedidoComIngressosEItens()
        {
            using var context = CriarContexto(nameof(AdicionarAsync_DevePersistirPedidoComIngressosEItens));
            var repository = new PedidoRepository(context);

            var produto = new ProdutoEntity { Nome = "Pipoca", Categoria = "Comida", Preco = 20m, QuantidadeEstoque = 10 };
            context.Produto.Add(produto);
            await context.SaveChangesAsync();

            var pedido = new PedidoEntity
            {
                SessaoId = 1,
                NomeCliente = "Gabriel",
                DataHoraPedido = DateTime.UtcNow,
                ValorTotal = 50m,
                StatusPedido = "Confirmado",
                Ingressos = [new IngressoEntity { SessaoId = 1, Fileira = "D", NumeroAssento = 12, PrecoPago = 30m }],
                Itens = [new ItemPedidoEntity { ProdutoId = produto.Id, Quantidade = 1, PrecoUnitario = 20m, Subtotal = 20m }]
            };

            var resultado = await repository.AdicionarAsync(pedido);

            var pedidoCompleto = await repository.ObterPorIdAsync(resultado.Id);

            Assert.NotNull(pedidoCompleto);
            Assert.Single(pedidoCompleto!.Ingressos!);
            Assert.Single(pedidoCompleto.Itens!);
        }

        [Fact]
        [Trait("Repository", "Pedido")]
        public async Task AtualizarStatusAsync_DeveAtualizarStatus_QuandoPedidoExiste()
        {
            using var context = CriarContexto(nameof(AtualizarStatusAsync_DeveAtualizarStatus_QuandoPedidoExiste));
            var repository = new PedidoRepository(context);

            var pedido = new PedidoEntity
            {
                SessaoId = 1,
                NomeCliente = "Gabriel",
                DataHoraPedido = DateTime.UtcNow,
                ValorTotal = 30m,
                StatusPedido = "Confirmado"
            };
            context.Pedido.Add(pedido);
            await context.SaveChangesAsync();

            var resultado = await repository.AtualizarStatusAsync(pedido.Id, "Cancelado");

            Assert.NotNull(resultado);
            Assert.Equal("Cancelado", resultado!.StatusPedido);
        }
    }
}
