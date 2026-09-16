namespace Bilheteria.API.Application.Dtos
{
    public class SessaoResponseDto
    {
        public int Id { get; set; }
        public int FilmeId { get; set; }
        public string? TituloFilme { get; set; }
        public DateTime DataHoraSessao { get; set; }
        public string Sala { get; set; } = string.Empty;
        public decimal PrecoIngresso { get; set; }
        public int CapacidadeTotal { get; set; }
        public int AssentosDisponiveis { get; set; }
    }
}
