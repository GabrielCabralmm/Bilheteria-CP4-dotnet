using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Domain.Entities;

namespace Bilheteria.API.Application.Mappers
{
    public static class FilmeMapper
    {
        public static FilmeEntity ToFilmeEntity(this FilmeRequestDto dto)
        {
            return new FilmeEntity
            {
                Titulo = dto.Titulo,
                Genero = dto.Genero,
                DuracaoMinutos = dto.DuracaoMinutos,
                ClassificacaoIndicativa = dto.ClassificacaoIndicativa,
                Sinopse = dto.Sinopse,
                EmCartaz = dto.EmCartaz
            };
        }

        public static FilmeResponseDto ToResponseDto(this FilmeEntity entity)
        {
            return new FilmeResponseDto
            {
                Id = entity.Id,
                Titulo = entity.Titulo,
                Genero = entity.Genero,
                DuracaoMinutos = entity.DuracaoMinutos,
                ClassificacaoIndicativa = entity.ClassificacaoIndicativa,
                Sinopse = entity.Sinopse,
                EmCartaz = entity.EmCartaz
            };
        }
    }
}
