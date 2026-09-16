using Bilheteria.API.Domain.Entities;
using Bilheteria.API.Domain.Interfaces;
using Bilheteria.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bilheteria.API.Infrastructure.Data.Repositories
{
    public class FilmeRepository : IFilmeRepository
    {
        private readonly ApplicationContext _context;

        public FilmeRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<FilmeEntity> Itens, int Total)> ObterTodosAsync(int deslocamento, int registroRetornado)
        {
            if (deslocamento < 0) deslocamento = 0;
            if (registroRetornado <= 0) registroRetornado = 10;

            var total = await _context.Filme.CountAsync();

            var itens = await _context.Filme
                .OrderBy(x => x.Id)
                .Skip(deslocamento)
                .Take(registroRetornado)
                .ToListAsync();

            return (itens, total);
        }

        public async Task<IEnumerable<FilmeEntity>> ObterEmCartazAsync()
        {
            return await _context.Filme
                .Where(x => x.EmCartaz)
                .OrderBy(x => x.Titulo)
                .ToListAsync();
        }

        public async Task<FilmeEntity?> ObterPorIdAsync(int id)
        {
            return await _context.Filme.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<FilmeEntity> AdicionarAsync(FilmeEntity entity)
        {
            _context.Filme.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<FilmeEntity?> EditarAsync(int id, FilmeEntity entity)
        {
            var filme = await _context.Filme.FirstOrDefaultAsync(x => x.Id == id);

            if (filme is null)
                return null;

            filme.Titulo = entity.Titulo;
            filme.Genero = entity.Genero;
            filme.DuracaoMinutos = entity.DuracaoMinutos;
            filme.ClassificacaoIndicativa = entity.ClassificacaoIndicativa;
            filme.Sinopse = entity.Sinopse;
            filme.EmCartaz = entity.EmCartaz;

            await _context.SaveChangesAsync();

            return filme;
        }

        public async Task<FilmeEntity?> DeletarAsync(int id)
        {
            var filme = await _context.Filme.FirstOrDefaultAsync(x => x.Id == id);

            if (filme is null)
                return null;

            _context.Filme.Remove(filme);
            await _context.SaveChangesAsync();

            return filme;
        }
    }
}
