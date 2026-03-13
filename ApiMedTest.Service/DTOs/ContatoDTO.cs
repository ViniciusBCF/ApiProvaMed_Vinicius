using ApiMedTest.Domain.Entities.Enums;
using System.Text.Json.Serialization;

namespace ApiMedTest.Service.DTOs
{
    public record class ContatoDTO
    {
        public string NomeContato { get; init; } = string.Empty;
        public DateTime DataNascimento { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Sexo Sexo { get; init; }
    }
}
