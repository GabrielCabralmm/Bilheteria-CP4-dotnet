using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bilheteria.API.Domain.Entities
{
    [Table("tb_produto")]
    [Index(nameof(Categoria))]
    public class ProdutoEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        [Column("c_nome")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Column("c_categoria")]
        public string Categoria { get; set; } = string.Empty;

        [Required]
        [Column("c_preco", TypeName = "NUMBER(10,2)")]
        public decimal Preco { get; set; }

        [Required]
        [Column("c_quantidade_estoque")]
        public int QuantidadeEstoque { get; set; }

        [JsonIgnore]
        public ICollection<ItemPedidoEntity>? ItensPedido { get; set; } = [];
    }
}
