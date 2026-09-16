using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bilheteria.API.Presentation.Controllers
{
    [Route("api/sessao")]
    [ApiController]
    public class SessaoController : ControllerBase
    {
        private readonly ISessaoUseCase _sessaoUseCase;

        public SessaoController(ISessaoUseCase sessaoUseCase)
        {
            _sessaoUseCase = sessaoUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as sessoes de exibicao",
            Description = """
            ## Informacoes do retorno
            * **Status 200:** lista paginada de sessoes, incluindo o titulo do filme e os assentos ainda disponiveis.
            * **Status 204:** nao ha sessoes cadastradas.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(PagedResultDto<SessaoResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma sessao cadastrada")]
        public async Task<IActionResult> Get(int deslocamento = 0, int registroRetornado = 10)
        {
            try
            {
                var resultado = await _sessaoUseCase.ObterTodosAsync(deslocamento, registroRetornado);

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
        [SwaggerOperation(Summary = "Obter uma sessao pelo Id")]
        [SwaggerResponse(statusCode: 200, description: "Sessao encontrada", type: typeof(SessaoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Sessao nao encontrada")]
        public async Task<IActionResult> Get([FromRoute, SwaggerParameter("Id da sessao")] int id)
        {
            var sessao = await _sessaoUseCase.ObterPorIdAsync(id);

            if (sessao is null)
                return NotFound();

            return Ok(sessao);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastrar uma nova sessao de exibicao para um filme")]
        [SwaggerResponse(statusCode: 201, description: "Sessao criada com sucesso", type: typeof(SessaoResponseDto))]
        [SwaggerResponse(statusCode: 400, description: "Dados invalidos")]
        public async Task<IActionResult> Post(SessaoRequestDto model)
        {
            try
            {
                var sessao = await _sessaoUseCase.AdicionarAsync(model);

                return CreatedAtAction(nameof(Get), new { id = sessao.Id }, sessao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Editar uma sessao existente")]
        [SwaggerResponse(statusCode: 200, description: "Sessao atualizada com sucesso", type: typeof(SessaoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Sessao nao encontrada")]
        public async Task<IActionResult> Put([FromRoute, SwaggerParameter("Id da sessao")] int id, SessaoRequestDto model)
        {
            try
            {
                var sessao = await _sessaoUseCase.EditarAsync(id, model);

                if (sessao is null)
                    return NotFound();

                return Ok(sessao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remover uma sessao")]
        [SwaggerResponse(statusCode: 200, description: "Sessao removida com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Sessao nao encontrada")]
        public async Task<IActionResult> Delete(int id)
        {
            var sessao = await _sessaoUseCase.DeletarAsync(id);

            if (sessao is null)
                return NotFound();

            return Ok(sessao);
        }
    }
}
