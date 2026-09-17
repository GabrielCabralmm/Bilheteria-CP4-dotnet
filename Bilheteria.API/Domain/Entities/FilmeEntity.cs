using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bilheteria.API.Domain.Entities
{
    [Table("tb_filme")]
    public class FilmeEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 1)]
        [Column("c_titulo")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(50)]
        [Column("c_genero")]
        public string? Genero { get; set; }

        [Column("c_duracao_minutos")]
        public int DuracaoMinutos { get; set; }

        [StringLength(10)]
        [Column("c_classificacao_indicativa")]
        public string? ClassificacaoIndicativa { get; set; }

        [StringLength(2000)]
        [Column("c_sinopse")]
        public string? Sinopse { get; set; }

        [Column("c_em_cartaz", TypeName = "NUMBER(1)")]
        public bool EmCartaz { get; set; }

        [JsonIgnore]
        public ICollection<SessaoEntity>? Sessoes { get; set; } = [];
    }
}