using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Bilheteria.API.Domain.Entities
{
    [Table("tb_sessao")]
    [Index(nameof(FilmeId), nameof(DataHoraSessao))]
    public class SessaoEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Filme))]
        [Column("c_filme_id")]
        public int FilmeId { get; set; }

        [Required]
        [Column("c_data_hora_sessao")]
        public DateTime DataHoraSessao { get; set; }

        [Required]
        [StringLength(20)]
        [Column("c_sala")]
        public string Sala { get; set; } = string.Empty;

        [Required]
        [Column("c_preco_ingresso", TypeName = "NUMBER(10,2)")]
        public decimal PrecoIngresso { get; set; }

        [Required]
        [Column("c_capacidade_total")]
        public int CapacidadeTotal { get; set; }

        [JsonIgnore]
        public FilmeEntity? Filme { get; set; }

        [JsonIgnore]
        public ICollection<PedidoEntity>? Pedidos { get; set; } = [];

        [JsonIgnore]
        public ICollection<IngressoEntity>? Ingressos { get; set; } = [];
    }
}
