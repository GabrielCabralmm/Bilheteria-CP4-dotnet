using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bilheteria.API.Domain.Entities
{
    [Table("tb_pedido")]
    [Index(nameof(DataHoraPedido))]
    public class PedidoEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Sessao))]
        [Column("c_sessao_id")]
        public int SessaoId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        [Column("c_nome_cliente")]
        public string NomeCliente { get; set; } = string.Empty;

        [Required]
        [Column("c_data_hora_pedido")]
        public DateTime DataHoraPedido { get; set; }

        [Required]
        [Column("c_valor_total", TypeName = "NUMBER(10,2)")]
        public decimal ValorTotal { get; set; }

        [Required]
        [StringLength(20)]
        [Column("c_status_pedido")]
        public string StatusPedido { get; set; } = "Confirmado";

        [JsonIgnore]
        public SessaoEntity? Sessao { get; set; }

        public ICollection<ItemPedidoEntity>? Itens { get; set; } = [];

        public ICollection<IngressoEntity>? Ingressos { get; set; } = [];
    }
}
