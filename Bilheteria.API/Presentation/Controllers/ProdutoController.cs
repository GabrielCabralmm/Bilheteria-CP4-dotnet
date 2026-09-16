using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bilheteria.API.Presentation.Controllers
{
    [Route("api/produto")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoUseCase _produtoUseCase;

        public ProdutoController(IProdutoUseCase produtoUseCase)
        {
            _produtoUseCase = produtoUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista os produtos do snack bar (comidas e bebidas)",
            Description = """
            ## Informacoes do retorno
            * **Status 200:** lista paginada de produtos.
            * **Status 204:** nao ha produtos cadastrados.
            """
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(PagedResultDto<ProdutoResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum produto cadastrado")]
        public async Task<IActionResult> Get(int deslocamento = 0, int registroRetornado = 10)
        {
            try
            {
                var resultado = await _produtoUseCase.ObterTodosAsync(deslocamento, registroRetornado);

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
        [SwaggerOperation(Summary = "Obter um produto pelo Id")]
        [SwaggerResponse(statusCode: 200, description: "Produto encontrado", type: typeof(ProdutoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Produto nao encontrado")]
        public async Task<IActionResult> Get([FromRoute, SwaggerParameter("Id do produto")] int id)
        {
            var produto = await _produtoUseCase.ObterPorIdAsync(id);

            if (produto is null)
                return NotFound();

            return Ok(produto);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastrar um novo produto no snack bar")]
        [SwaggerResponse(statusCode: 201, description: "Produto criado com sucesso", type: typeof(ProdutoResponseDto))]
        [SwaggerResponse(statusCode: 400, description: "Dados invalidos")]
        public async Task<IActionResult> Post(ProdutoRequestDto model)
        {
            try
            {
                var produto = await _produtoUseCase.AdicionarAsync(model);

                return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Editar um produto existente")]
        [SwaggerResponse(statusCode: 200, description: "Produto atualizado com sucesso", type: typeof(ProdutoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Produto nao encontrado")]
        public async Task<IActionResult> Put([FromRoute, SwaggerParameter("Id do produto")] int id, ProdutoRequestDto model)
        {
            try
            {
                var produto = await _produtoUseCase.EditarAsync(id, model);

                if (produto is null)
                    return NotFound();

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remover um produto")]
        [SwaggerResponse(statusCode: 200, description: "Produto removido com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Produto nao encontrado")]
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _produtoUseCase.DeletarAsync(id);

            if (produto is null)
                return NotFound();

            return Ok(produto);
        }
    }
}
