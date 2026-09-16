using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Application.Interfaces;
using Bilheteria.API.Application.Mappers;
using Bilheteria.API.Domain.Entities;
using Bilheteria.API.Domain.Exceptions;
using Bilheteria.API.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bilheteria.API.Application.UseCases
{
    public class PedidoUseCase : IPedidoUseCase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ISessaoRepository _sessaoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly ILogger<PedidoUseCase> _logger;

        public PedidoUseCase(
            IPedidoRepository pedidoRepository,
            ISessaoRepository sessaoRepository,
            IProdutoRepository produtoRepository,
            ILogger<PedidoUseCase> logger)
        {
            _pedidoRepository = pedidoRepository;
            _sessaoRepository = sessaoRepository;
            _produtoRepository = produtoRepository;
            _logger = logger;
        }

        public async Task<PagedResultDto<PedidoResponseDto>> ObterTodosAsync(int deslocamento, int registroRetornado)
        {
            var (itens, total) = await _pedidoRepository.ObterTodosAsync(deslocamento, registroRetornado);

            return new PagedResultDto<PedidoResponseDto>
            {
                Itens = itens.Select(x => x.ToResponseDto()),
                TotalRegistros = total,
                Deslocamento = deslocamento,
                RegistroRetornado = registroRetornado
            };
        }

        public async Task<PedidoResponseDto?> ObterPorIdAsync(int id)
        {
            var pedido = await _pedidoRepository.ObterPorIdAsync(id);

            return pedido?.ToResponseDto();
        }

        public async Task<PedidoResponseDto> CriarPedidoAsync(PedidoRequestDto dto)
        {
            var sessao = await _sessaoRepository.ObterPorIdAsync(dto.SessaoId)
                ?? throw new RegistroNaoEncontradoException("Sessao", dto.SessaoId);

            var vendidos = await _pedidoRepository.ContarIngressosVendidosAsync(dto.SessaoId);
            var disponiveis = sessao.CapacidadeTotal - vendidos;

            if (dto.Ingressos.Count > disponiveis)
            {
                _logger.LogWarning(
                    "Checkout recusado: sessao {SessaoId} tem apenas {Disponiveis} assentos disponiveis para {Solicitados} solicitados",
                    dto.SessaoId, disponiveis, dto.Ingressos.Count);

                throw new CapacidadeExcedidaException(sessao.CapacidadeTotal, disponiveis);
            }

            var assentosSolicitados = new HashSet<(string Fileira, int NumeroAssento)>();
            var ingressosEntity = new List<IngressoEntity>();

            foreach (var ingressoDto in dto.Ingressos)
            {
                var chave = (ingressoDto.Fileira.ToUpperInvariant(), ingressoDto.NumeroAssento);

                if (!assentosSolicitados.Add(chave))
                {
                    _logger.LogWarning(
                        "Checkout recusado: assento {Fileira}{NumeroAssento} informado duas vezes no mesmo pedido",
                        ingressoDto.Fileira, ingressoDto.NumeroAssento);

                    throw new AssentoIndisponivelException(ingressoDto.Fileira, ingressoDto.NumeroAssento);
                }

                var ocupado = await _pedidoRepository.AssentoOcupadoAsync(dto.SessaoId, ingressoDto.Fileira, ingressoDto.NumeroAssento);

                if (ocupado)
                {
                    _logger.LogWarning(
                        "Checkout recusado: assento {Fileira}{NumeroAssento} ja vendido na sessao {SessaoId}",
                        ingressoDto.Fileira, ingressoDto.NumeroAssento, dto.SessaoId);

                    throw new AssentoIndisponivelException(ingressoDto.Fileira, ingressoDto.NumeroAssento);
                }

                ingressosEntity.Add(new IngressoEntity
                {
                    SessaoId = dto.SessaoId,
                    Fileira = ingressoDto.Fileira,
                    NumeroAssento = ingressoDto.NumeroAssento,
                    PrecoPago = sessao.PrecoIngresso
                });
            }

            var produtosCache = new Dictionary<int, ProdutoEntity>();
            var itensEntity = new List<ItemPedidoEntity>();
            var totalItens = 0m;

            foreach (var itemDto in dto.Itens ?? [])
            {
                if (!produtosCache.TryGetValue(itemDto.ProdutoId, out var produto))
                {
                    produto = await _produtoRepository.ObterPorIdAsync(itemDto.ProdutoId)
                        ?? throw new RegistroNaoEncontradoException("Produto", itemDto.ProdutoId);

                    produtosCache[itemDto.ProdutoId] = produto;
                }

                if (produto.QuantidadeEstoque < itemDto.Quantidade)
                {
                    _logger.LogWarning(
                        "Checkout recusado: estoque insuficiente para o produto {ProdutoId} ({Estoque} em estoque, {Solicitado} solicitado)",
                        produto.Id, produto.QuantidadeEstoque, itemDto.Quantidade);

                    throw new EstoqueInsuficienteException(produto.Nome, produto.QuantidadeEstoque);
                }

                produto.QuantidadeEstoque -= itemDto.Quantidade;

                var subtotal = produto.Preco * itemDto.Quantidade;
                totalItens += subtotal;

                itensEntity.Add(new ItemPedidoEntity
                {
                    ProdutoId = itemDto.ProdutoId,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.Preco,
                    Subtotal = subtotal
                });
            }

            foreach (var produtoAtualizado in produtosCache.Values)
            {
                await _produtoRepository.EditarAsync(produtoAtualizado.Id, produtoAtualizado);
            }

            var valorTotal = (ingressosEntity.Count * sessao.PrecoIngresso) + totalItens;

            var pedido = new PedidoEntity
            {
                SessaoId = dto.SessaoId,
                NomeCliente = dto.NomeCliente,
                DataHoraPedido = DateTime.UtcNow,
                ValorTotal = valorTotal,
                StatusPedido = "Confirmado",
                Ingressos = ingressosEntity,
                Itens = itensEntity
            };

            var pedidoCriado = await _pedidoRepository.AdicionarAsync(pedido);
            var pedidoCompleto = await _pedidoRepository.ObterPorIdAsync(pedidoCriado.Id);

            _logger.LogInformation(
                "Pedido {PedidoId} confirmado para a sessao {SessaoId}: {QuantidadeIngressos} ingresso(s), valor total {ValorTotal}",
                pedidoCriado.Id, dto.SessaoId, ingressosEntity.Count, valorTotal);

            return (pedidoCompleto ?? pedidoCriado).ToResponseDto();
        }

        public async Task<PedidoResponseDto?> CancelarAsync(int id)
        {
            var pedido = await _pedidoRepository.AtualizarStatusAsync(id, "Cancelado");

            return pedido?.ToResponseDto();
        }
    }
}
