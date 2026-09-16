namespace Bilheteria.API.Application.Dtos
{
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Itens { get; set; } = [];
        public int TotalRegistros { get; set; }
        public int Deslocamento { get; set; }
        public int RegistroRetornado { get; set; }
    }
}
