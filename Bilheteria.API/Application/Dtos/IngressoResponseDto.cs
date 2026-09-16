namespace Bilheteria.API.Application.Dtos
{
    public class IngressoResponseDto
    {
        public int Id { get; set; }
        public string Fileira { get; set; } = string.Empty;
        public int NumeroAssento { get; set; }
        public decimal PrecoPago { get; set; }
    }
}
