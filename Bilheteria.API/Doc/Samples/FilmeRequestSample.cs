using Bilheteria.API.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Bilheteria.API.Doc.Samples
{
    public class FilmeRequestSample : IExamplesProvider<FilmeRequestDto>
    {
        public FilmeRequestDto GetExamples()
        {
            return new FilmeRequestDto
            {
                Titulo = "Duna: Parte Dois",
                Genero = "Ficcao Cientifica",
                DuracaoMinutos = 166,
                ClassificacaoIndicativa = "14 anos",
                Sinopse = "Paul Atreides se une a Chani e aos Fremen em busca de vinganca.",
                EmCartaz = true
            };
        }
    }
}
