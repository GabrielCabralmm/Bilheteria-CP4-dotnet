using Bilheteria.API.Domain.Entities;
using Bilheteria.API.Domain.Interfaces;
using Bilheteria.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bilheteria.API.Infrastructure.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ApplicationContext _context;

        public PedidoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<PedidoEntity> Itens, int Total)> ObterTodosAsync(int deslocamento, int registroRetornado)
        {
            if (deslocamento < 0) deslocamento = 0;
            if (registroRetornado <= 0) registroRetornado = 10;

            var total = await _context.Pedido.CountAsync();

            var itens = await _context.Pedido
                .Include(x => x.Ingressos)
                .Include(x => x.Itens)
                    .ThenInclude(i => i.Produto)
                .OrderByDescending(x => x.DataHoraPedido)
                .Skip(deslocamento)
                .Take(registroRetornado)
                .ToListAsync();

            return (itens, total);
        }

        public async Task<PedidoEntity?> ObterPorIdAsync(int id)
        {
            return await _context.Pedido
                .Include(x => x.Ingressos)
                .Include(x => x.Itens)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> AssentoOcupadoAsync(int sessaoId, string fileira, int numeroAssento)
        {
            var quantidade = await _context.Ingresso.CountAsync(x =>
                x.SessaoId == sessaoId &&
                x.Fileira == fileira &&
                x.NumeroAssento == numeroAssento);

            return quantidade > 0;
        }

        public async Task<int> ContarIngressosVendidosAsync(int sessaoId)
        {
            return await _context.Ingresso.CountAsync(x => x.SessaoId == sessaoId);
        }

        public async Task<PedidoEntity> AdicionarAsync(PedidoEntity entity)
        {
            _context.Pedido.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<PedidoEntity?> AtualizarStatusAsync(int id, string status)
        {
            var pedido = await _context.Pedido
                .Include(x => x.Ingressos)
                .Include(x => x.Itens)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pedido is null)
                return null;

            pedido.StatusPedido = status;

            await _context.SaveChangesAsync();

            return pedido;
        }
    }
}
