using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bilheteria.API.Domain.Entities
{
    [Table("tb_item_pedido")]
    [Index(nameof(PedidoId), nameof(ProdutoId))]
    public class ItemPedidoEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Pedido))]
        [Column("c_pedido_id")]
        public int PedidoId { get; set; }

        [ForeignKey(nameof(Produto))]
        [Column("c_produto_id")]
        public int ProdutoId { get; set; }

        [Required]
        [Column("c_quantidade")]
        public int Quantidade { get; set; }

        [Required]
        [Column("c_preco_unitario", TypeName = "NUMBER(10,2)")]
        public decimal PrecoUnitario { get; set; }

        [Required]
        [Column("c_subtotal", TypeName = "NUMBER(10,2)")]
        public decimal Subtotal { get; set; }

        [JsonIgnore]
        public PedidoEntity? Pedido { get; set; }

        public ProdutoEntity? Produto { get; set; }
    }
}
