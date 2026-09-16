namespace Bilheteria.API.Application.Dtos
{
    public class FilmeResponseDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Genero { get; set; }
        public int DuracaoMinutos { get; set; }
        public string? ClassificacaoIndicativa { get; set; }
        public string? Sinopse { get; set; }
        public bool EmCartaz { get; set; }
    }
}
