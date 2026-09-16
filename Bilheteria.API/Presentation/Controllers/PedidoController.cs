using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Application.Interfaces;
using Bilheteria.API.Doc.Samples;
using Bilheteria.API.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Bilheteria.API.Presentation.Controllers
{
    [Route("api/pedido")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoUseCase _pedidoUseCase;

        public PedidoController(IPedidoUseCase pedidoUseCase)
        {
            _pedidoUseCase = pedidoUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todos os pedidos",
            Description = """
            ## Informacoes do retorno
            * **Status 200:** lista paginada de pedidos, com os ingressos e itens do snack bar de cada um.
            * **Status 204:** nao ha pedidos cadastrados.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(PagedResultDto<PedidoResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum pedido cadastrado")]
        public async Task<IActionResult> Get(int deslocamento = 0, int registroRetornado = 10)
        {
            try
            {
                var resultado = await _pedidoUseCase.ObterTodosAsync(deslocamento, registroRetornado);

                if (!resultado.Itens.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obter um pedido pelo Id")]
        [SwaggerResponse(statusCode: 200, description: "Pedido encontrado", type: typeof(PedidoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Pedido nao encontrado")]
        public async Task<IActionResult> Get([FromRoute, SwaggerParameter("Id do pedido")] int id)
        {
            var pedido = await _pedidoUseCase.ObterPorIdAsync(id);

            if (pedido is null)
                return NotFound();

            return Ok(pedido);
        }

        [HttpPost]
        [EnableRateLimiting("politica_checkout")]
        [SwaggerOperation(
            Summary = "Realizar a compra (checkout): ingressos com assento marcado e, opcionalmente, itens do snack bar",
            Description = """
            ## Regras de negocio aplicadas
            * **Status 201:** pedido confirmado, com o valor total ja calculado.
            * **Status 404:** a sessao ou algum produto informado nao existe.
            * **Status 409:** algum assento informado ja foi vendido para essa sessao.
            * **Status 422:** a sessao nao tem assentos suficientes, ou algum produto nao tem estoque suficiente.
            * **Status 429:** limite de tentativas de compra excedido (rate limiting).
            """
        )]
        [SwaggerRequestExample(typeof(PedidoRequestDto), typeof(PedidoRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Pedido confirmado com sucesso", type: typeof(PedidoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Sessao ou produto nao encontrado")]
        [SwaggerResponse(statusCode: 409, description: "Assento ja vendido para essa sessao")]
        [SwaggerResponse(statusCode: 422, description: "Capacidade da sessao ou estoque de produto insuficiente")]
        [SwaggerResponse(statusCode: 429, description: "Numero de tentativas excedido")]
        public async Task<IActionResult> Post(PedidoRequestDto model)
        {
            try
            {
                var pedido = await _pedidoUseCase.CriarPedidoAsync(model);

                return CreatedAtAction(nameof(Get), new { id = pedido.Id }, pedido);
            }
            catch (RegistroNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AssentoIndisponivelException ex)
            {
                return Conflict(ex.Message);
            }
            catch (CapacidadeExcedidaException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (EstoqueInsuficienteException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/cancelar")]
        [SwaggerOperation(Summary = "Cancelar um pedido")]
        [SwaggerResponse(statusCode: 200, description: "Pedido cancelado com sucesso", type: typeof(PedidoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Pedido nao encontrado")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var pedido = await _pedidoUseCase.CancelarAsync(id);

            if (pedido is null)
                return NotFound();

            return Ok(pedido);
        }
    }
}
