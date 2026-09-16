using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Domain.Entities;

namespace Bilheteria.API.Application.Mappers
{
    public static class ProdutoMapper
    {
        public static ProdutoEntity ToProdutoEntity(this ProdutoRequestDto dto)
        {
            return new ProdutoEntity
            {
                Nome = dto.Nome,
                Categoria = dto.Categoria,
                Preco = dto.Preco,
                QuantidadeEstoque = dto.QuantidadeEstoque
            };
        }

        public static ProdutoResponseDto ToResponseDto(this ProdutoEntity entity)
        {
            return new ProdutoResponseDto
            {
                Id = entity.Id,
                Nome = entity.Nome,
                Categoria = entity.Categoria,
                Preco = entity.Preco,
                QuantidadeEstoque = entity.QuantidadeEstoque
            };
        }
    }
}
