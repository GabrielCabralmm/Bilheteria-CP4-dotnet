using Bilheteria.API.Application.Dtos;
using Bilheteria.API.Domain.Entities;

namespace Bilheteria.API.Application.Mappers
{
    public static class SessaoMapper
    {
        public static SessaoEntity ToSessaoEntity(this SessaoRequestDto dto)
        {
            return new SessaoEntity
            {
                FilmeId = dto.FilmeId,
                DataHoraSessao = dto.DataHoraSessao,
                Sala = dto.Sala,
                PrecoIngresso = dto.PrecoIngresso,
                CapacidadeTotal = dto.CapacidadeTotal
            };
        }

        public static SessaoResponseDto ToResponseDto(this SessaoEntity entity, int assentosDisponiveis)
        {
            return new SessaoResponseDto
            {
                Id = entity.Id,
                FilmeId = entity.FilmeId,
                TituloFilme = entity.Filme?.Titulo,
                DataHoraSessao = entity.DataHoraSessao,
                Sala = entity.Sala,
                PrecoIngresso = entity.PrecoIngresso,
                CapacidadeTotal = entity.CapacidadeTotal,
                AssentosDisponiveis = assentosDisponiveis
            };
        }
    }
}
