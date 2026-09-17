using Bilheteria.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bilheteria.API.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<FilmeEntity> Filme { get; set; }
        public DbSet<SessaoEntity> Sessao { get; set; }
        public DbSet<ProdutoEntity> Produto { get; set; }
        public DbSet<PedidoEntity> Pedido { get; set; }
        public DbSet<ItemPedidoEntity> ItemPedido { get; set; }
        public DbSet<IngressoEntity> Ingresso { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FilmeEntity>()
                .Property(f => f.EmCartaz)
                .HasConversion<int>();
        }
    }
}