using Bilheteria.API.Domain.Entities;
using Bilheteria.API.Domain.Interfaces;
using Bilheteria.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bilheteria.API.Infrastructure.Data.Repositories
{
    public class SessaoRepository : ISessaoRepository
    {
        private readonly ApplicationContext _context;

        public SessaoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<SessaoEntity> Itens, int Total)> ObterTodosAsync(int deslocamento, int registroRetornado)
        {
            if (deslocamento < 0) deslocamento = 0;
            if (registroRetornado <= 0) registroRetornado = 10;

            var total = await _context.Sessao.CountAsync();

            var itens = await _context.Sessao
                .Include(x => x.Filme)
                .OrderBy(x => x.DataHoraSessao)
                .Skip(deslocamento)
                .Take(registroRetornado)
                .ToListAsync();

            return (itens, total);
        }

        public async Task<SessaoEntity?> ObterPorIdAsync(int id)
        {
            return await _context.Sessao
                .Include(x => x.Filme)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SessaoEntity> AdicionarAsync(SessaoEntity entity)
        {
            _context.Sessao.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<SessaoEntity?> EditarAsync(int id, SessaoEntity entity)
        {
            var sessao = await _context.Sessao.FirstOrDefaultAsync(x => x.Id == id);

            if (sessao is null)
                return null;

            sessao.FilmeId = entity.FilmeId;
            sessao.DataHoraSessao = entity.DataHoraSessao;
            sessao.Sala = entity.Sala;
            sessao.PrecoIngresso = entity.PrecoIngresso;
            sessao.CapacidadeTotal = entity.CapacidadeTotal;

            await _context.SaveChangesAsync();

            return sessao;
        }

        public async Task<SessaoEntity?> DeletarAsync(int id)
        {
            var sessao = await _context.Sessao.FirstOrDefaultAsync(x => x.Id == id);

            if (sessao is null)
                return null;

            _context.Sessao.Remove(sessao);
            await _context.SaveChangesAsync();

            return sessao;
        }
    }
}
