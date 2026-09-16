using Bilheteria.API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Bilheteria.API.Doc.Samples
{
    public class PedidoRequestSample : IExamplesProvider<PedidoRequestDto>
    {
        public PedidoRequestDto GetExamples()
        {
            return new PedidoRequestDto
            {
                SessaoId = 1,
                NomeCliente = "Gabriel Mariano",
                Ingressos =
                [
                    new IngressoRequestDto { Fileira = "D", NumeroAssento = 12 },
                    new IngressoRequestDto { Fileira = "D", NumeroAssento = 13 }
                ],
                Itens =
                [
                    new ItemPedidoRequestDto { ProdutoId = 1, Quantidade = 1 },
                    new ItemPedidoRequestDto { ProdutoId = 2, Quantidade = 2 }
                ]
            };
        }
    }
}
