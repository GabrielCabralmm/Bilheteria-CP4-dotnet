using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bilheteria.API.Domain.Entities
{
    [Table("tb_ingresso")]
    [Index(nameof(SessaoId), nameof(Fileira), nameof(NumeroAssento), IsUnique = true, Name = "IDX_ASSENTO_SESSAO")]
    public class IngressoEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Pedido))]
        [Column("c_pedido_id")]
        public int PedidoId { get; set; }

        [ForeignKey(nameof(Sessao))]
        [Column("c_sessao_id")]
        public int SessaoId { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 1)]
        [Column("c_fileira")]
        public string Fileira { get; set; } = string.Empty;

        [Required]
        [Column("c_numero_assento")]
        public int NumeroAssento { get; set; }

        [Required]
        [Column("c_preco_pago", TypeName = "NUMBER(10,2)")]
        public decimal PrecoPago { get; set; }

        [JsonIgnore]
        public PedidoEntity? Pedido { get; set; }

        [JsonIgnore]
        public SessaoEntity? Sessao { get; set; }
    }
}
