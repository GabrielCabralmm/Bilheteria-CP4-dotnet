using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Domain.Entities;

namespace Bilheteria.API.Application.Mappers
{
    public static class PedidoMapper
    {
        public static PedidoResponseDto ToResponseDto(this PedidoEntity entity)
        {
            return new PedidoResponseDto
            {
                Id = entity.Id,
                SessaoId = entity.SessaoId,
                NomeCliente = entity.NomeCliente,
                DataHoraPedido = entity.DataHoraPedido,
                ValorTotal = entity.ValorTotal,
                StatusPedido = entity.StatusPedido,
                Ingressos = entity.Ingressos?
                    .Select(i => new IngressoResponseDto
                    {
                        Id = i.Id,
                        Fileira = i.Fileira,
                        NumeroAssento = i.NumeroAssento,
                        PrecoPago = i.PrecoPago
                    })
                    .ToList() ?? [],
                Itens = entity.Itens?
                    .Select(i => new ItemPedidoResponseDto
                    {
                        Id = i.Id,
                        ProdutoId = i.ProdutoId,
                        NomeProduto = i.Produto?.Nome,
                        Quantidade = i.Quantidade,
                        PrecoUnitario = i.PrecoUnitario,
                        Subtotal = i.Subtotal
                    })
                    .ToList() ?? []
            };
        }
    }
}
