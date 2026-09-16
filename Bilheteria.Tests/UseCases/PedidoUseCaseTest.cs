using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Application.UseCases;
using Bilheteria.API.Domain.Entities;
using Bilheteria.API.Domain.Exceptions;
using Bilheteria.API.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Bilheteria.Tests.UseCases
{
    public class PedidoUseCaseTest
    {
        private readonly Mock<IPedidoRepository> _pedidoRepository;
        private readonly Mock<ISessaoRepository> _sessaoRepository;
        private readonly Mock<IProdutoRepository> _produtoRepository;
        private readonly Mock<ILogger<PedidoUseCase>> _logger;
        private readonly PedidoUseCase _pedidoUseCase;

        public PedidoUseCaseTest()
        {
            _pedidoRepository = new Mock<IPedidoRepository>();
            _sessaoRepository = new Mock<ISessaoRepository>();
            _produtoRepository = new Mock<IProdutoRepository>();
            _logger = new Mock<ILogger<PedidoUseCase>>();

            _pedidoUseCase = new PedidoUseCase(
                _pedidoRepository.Object,
                _sessaoRepository.Object,
                _produtoRepository.Object,
                _logger.Object);
        }

        private static SessaoEntity SessaoPadrao() => new()
        {
            Id = 1,
            FilmeId = 1,
            Sala = "Sala 1",
            PrecoIngresso = 30m,
            CapacidadeTotal = 50,
            DataHoraSessao = DateTime.UtcNow.AddHours(2)
        };

        [Fact]
        [Trait("UseCase", "Pedido")]
        public async Task CriarPedidoAsync_DeveConfirmarPedido_QuandoDadosValidos()
        {
            var sessao = SessaoPadrao();
            var produto = new ProdutoEntity { Id = 1, Nome = "Pipoca", Categoria = "Comida", Preco = 20m, QuantidadeEstoque = 10 };

            _sessaoRepository.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(sessao);
            _pedidoRepository.Setup(r => r.ContarIngressosVendidosAsync(1)).ReturnsAsync(0);
            _pedidoRepository.Setup(r => r.AssentoOcupadoAsync(1, "D", 12)).ReturnsAsync(false);
            _produtoRepository.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(produto);
            _pedidoRepository
                .Setup(r => r.AdicionarAsync(It.IsAny<PedidoEntity>()))
                .ReturnsAsync((PedidoEntity p) => { p.Id = 100; return p; });
            _pedidoRepository
                .Setup(r => r.ObterPorIdAsync(100))
                .ReturnsAsync(new PedidoEntity
                {
                    Id = 100,
                    SessaoId = 1,
                    NomeCliente = "Gabriel",
                    ValorTotal = 50m,
                    StatusPedido = "Confirmado",
                    Ingressos = [new IngressoEntity { Id = 1, Fileira = "D", NumeroAssento = 12, PrecoPago = 30m }],
                    Itens = [new ItemPedidoEntity { Id = 1, ProdutoId = 1, Quantidade = 1, PrecoUnitario = 20m, Subtotal = 20m }]
                });

            var dto = new PedidoRequestDto
            {
                SessaoId = 1,
                NomeCliente = "Gabriel",
                Ingressos = [new IngressoRequestDto { Fileira = "D", NumeroAssento = 12 }],
                Itens = [new ItemPedidoRequestDto { ProdutoId = 1, Quantidade = 1 }]
            };

            var resultado = await _pedidoUseCase.CriarPedidoAsync(dto);

            Assert.Equal(100, resultado.Id);
            Assert.Equal(50m, resultado.ValorTotal);
            Assert.Single(resultado.Ingressos);
            Assert.Single(resultado.Itens);
        }

        [Fact]
        [Trait("UseCase", "Pedido")]
        public async Task CriarPedidoAsync_DeveLancarExcecao_QuandoSessaoNaoExiste()
        {
            _sessaoRepository.Setup(r => r.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((SessaoEntity?)null);

            var dto = new PedidoRequestDto
            {
                SessaoId = 99,
                NomeCliente = "Gabriel",
                Ingressos = [new IngressoRequestDto { Fileira = "A", NumeroAssento = 1 }]
            };

            await Assert.ThrowsAsync<RegistroNaoEncontradoException>(() => _pedidoUseCase.CriarPedidoAsync(dto));
        }

        [Fact]
        [Trait("UseCase", "Pedido")]
        public async Task CriarPedidoAsync_DeveLancarExcecao_QuandoAssentoJaFoiVendido()
        {
            var sessao = SessaoPadrao();

            _sessaoRepository.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(sessao);
            _pedidoRepository.Setup(r => r.ContarIngressosVendidosAsync(1)).ReturnsAsync(1);
            _pedidoRepository.Setup(r => r.AssentoOcupadoAsync(1, "D", 12)).ReturnsAsync(true);

            var dto = new PedidoRequestDto
            {
                SessaoId = 1,
                NomeCliente = "Gabriel",
                Ingressos = [new IngressoRequestDto { Fileira = "D", NumeroAssento = 12 }]
            };

            await Assert.ThrowsAsync<AssentoIndisponivelException>(() => _pedidoUseCase.CriarPedidoAsync(dto));
        }

        [Fact]
        [Trait("UseCase", "Pedido")]
        public async Task CriarPedidoAsync_DeveLancarExcecao_QuandoMesmoAssentoRepetidoNoPedido()
        {
            var sessao = SessaoPadrao();

            _sessaoRepository.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(sessao);
            _pedidoRepository.Setup(r => r.ContarIngressosVendidosAsync(1)).ReturnsAsync(0);
            _pedidoRepository.Setup(r => r.AssentoOcupadoAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(false);

            var dto = new PedidoRequestDto
            {
                SessaoId = 1,
                NomeCliente = "Gabriel",
                Ingressos =
                [
                    new IngressoRequestDto { Fileira = "D", NumeroAssento = 12 },
                    new IngressoRequestDto { Fileira = "D", NumeroAssento = 12 }
                ]
            };

            await Assert.ThrowsAsync<AssentoIndisponivelException>(() => _pedidoUseCase.CriarPedidoAsync(dto));
        }

        [Fact]
        [Trait("UseCase", "Pedido")]
        public async Task CriarPedidoAsync_DeveLancarExcecao_QuandoCapacidadeExcedida()
        {
            var sessao = SessaoPadrao();
            sessao.CapacidadeTotal = 1;

            _sessaoRepository.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(sessao);
            _pedidoRepository.Setup(r => r.ContarIngressosVendidosAsync(1)).ReturnsAsync(1);

            var dto = new PedidoRequestDto
            {
                SessaoId = 1,
                NomeCliente = "Gabriel",
                Ingressos = [new IngressoRequestDto { Fileira = "D", NumeroAssento = 12 }]
            };

            await Assert.ThrowsAsync<CapacidadeExcedidaException>(() => _pedidoUseCase.CriarPedidoAsync(dto));
        }

        [Fact]
        [Trait("UseCase", "Pedido")]
        public async Task CriarPedidoAsync_DeveLancarExcecao_QuandoEstoqueInsuficiente()
        {
            var sessao = SessaoPadrao();
            var produto = new ProdutoEntity { Id = 1, Nome = "Refrigerante", Categoria = "Bebida", Preco = 10m, QuantidadeEstoque = 1 };

            _sessaoRepository.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(sessao);
            _pedidoRepository.Setup(r => r.ContarIngressosVendidosAsync(1)).ReturnsAsync(0);
            _pedidoRepository.Setup(r => r.AssentoOcupadoAsync(1, "D", 12)).ReturnsAsync(false);
            _produtoRepository.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(produto);

            var dto = new PedidoRequestDto
            {
                SessaoId = 1,
                NomeCliente = "Gabriel",
                Ingressos = [new IngressoRequestDto { Fileira = "D", NumeroAssento = 12 }],
                Itens = [new ItemPedidoRequestDto { ProdutoId = 1, Quantidade = 5 }]
            };

            await Assert.ThrowsAsync<EstoqueInsuficienteException>(() => _pedidoUseCase.CriarPedidoAsync(dto));
        }
    }
}
