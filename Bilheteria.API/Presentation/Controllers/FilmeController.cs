using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Application.Interfaces;
using Bilheteria.API.Doc.Samples;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Bilheteria.API.Presentation.Controllers
{
    [Route("api/filme")]
    [ApiController]
    public class FilmeController : ControllerBase
    {
        private readonly IFilmeUseCase _filmeUseCase;

        public FilmeController(IFilmeUseCase filmeUseCase)
        {
            _filmeUseCase = filmeUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todos os filmes",
            Description = """
            ## Informacoes do retorno
            * **Status 200:** lista paginada de filmes.
            * **Status 204:** nao ha filmes cadastrados.

            Use `deslocamento` e `registroRetornado` para paginar o resultado.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(PagedResultDto<FilmeResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum filme cadastrado")]
        public async Task<IActionResult> Get(int deslocamento = 0, int registroRetornado = 10)
        {
            try
            {
                var resultado = await _filmeUseCase.ObterTodosAsync(deslocamento, registroRetornado);

                if (!resultado.Itens.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("em-cartaz")]
        [SwaggerOperation(Summary = "Lista apenas os filmes que estao em cartaz")]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(IEnumerable<FilmeResponseDto>))]
        public async Task<IActionResult> GetEmCartaz()
        {
            try
            {
                var resultado = await _filmeUseCase.ObterEmCartazAsync();

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obter um filme pelo Id")]
        [SwaggerResponse(statusCode: 200, description: "Filme encontrado", type: typeof(FilmeResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Filme nao encontrado")]
        public async Task<IActionResult> Get([FromRoute, SwaggerParameter("Id do filme")] int id)
        {
            var filme = await _filmeUseCase.ObterPorIdAsync(id);

            if (filme is null)
                return NotFound();

            return Ok(filme);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastrar um novo filme")]
        [SwaggerRequestExample(typeof(FilmeRequestDto), typeof(FilmeRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Filme criado com sucesso", type: typeof(FilmeResponseDto))]
        [SwaggerResponse(statusCode: 400, description: "Dados invalidos")]
        public async Task<IActionResult> Post(FilmeRequestDto model)
        {
            try
            {
                var filme = await _filmeUseCase.AdicionarAsync(model);

                return CreatedAtAction(nameof(Get), new { id = filme.Id }, filme);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Editar um filme existente")]
        [SwaggerResponse(statusCode: 200, description: "Filme atualizado com sucesso", type: typeof(FilmeResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Filme nao encontrado")]
        public async Task<IActionResult> Put([FromRoute, SwaggerParameter("Id do filme")] int id, FilmeRequestDto model)
        {
            try
            {
                var filme = await _filmeUseCase.EditarAsync(id, model);

                if (filme is null)
                    return NotFound();

                return Ok(filme);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remover um filme")]
        [SwaggerResponse(statusCode: 200, description: "Filme removido com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Filme nao encontrado")]
        public async Task<IActionResult> Delete(int id)
        {
            var filme = await _filmeUseCase.DeletarAsync(id);

            if (filme is null)
                return NotFound();

            return Ok(filme);
        }
    }
}
