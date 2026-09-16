using Bilheteria.API.Domain.Entities;
using Bilheteria.API.Domain.Interfaces;
using Bilheteria.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bilheteria.API.Infrastructure.Data.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApplicationContext _context;

        public ProdutoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<ProdutoEntity> Itens, int Total)> ObterTodosAsync(int deslocamento, int registroRetornado)
        {
            if (deslocamento < 0) deslocamento = 0;
            if (registroRetornado <= 0) registroRetornado = 10;

            var total = await _context.Produto.CountAsync();

            var itens = await _context.Produto
                .OrderBy(x => x.Id)
                .Skip(deslocamento)
                .Take(registroRetornado)
                .ToListAsync();

            return (itens, total);
        }

        public async Task<ProdutoEntity?> ObterPorIdAsync(int id)
        {
            return await _context.Produto.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ProdutoEntity> AdicionarAsync(ProdutoEntity entity)
        {
            _context.Produto.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<ProdutoEntity?> EditarAsync(int id, ProdutoEntity entity)
        {
            var produto = await _context.Produto.FirstOrDefaultAsync(x => x.Id == id);

            if (produto is null)
                return null;

            produto.Nome = entity.Nome;
            produto.Categoria = entity.Categoria;
            produto.Preco = entity.Preco;
            produto.QuantidadeEstoque = entity.QuantidadeEstoque;

            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task<ProdutoEntity?> DeletarAsync(int id)
        {
            var produto = await _context.Produto.FirstOrDefaultAsync(x => x.Id == id);

            if (produto is null)
                return null;

            _context.Produto.Remove(produto);
            await _context.SaveChangesAsync();

            return produto;
        }
    }
}
