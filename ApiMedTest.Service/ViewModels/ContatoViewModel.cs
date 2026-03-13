using ApiMedTest.Domain.Entities;
using ApiMedTest.Domain.Entities.Enums;
using System.Text.Json.Serialization;

namespace ApiMedTest.Service.ViewModels
{
    public class ContatoViewModel
    {
        public Guid Id { get; init; }
        public string NomeContato { get; init; } = string.Empty;
        public DateTime DataNascimento { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Sexo Sexo { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusContato StatusContato { get; init; } = StatusContato.Ativo;

        public int Idade => Contato.CalcularIdade(DataNascimento);

        public DateTime DataCriacao { get; init; }
        public DateTime? DataAtualizacao { get; init; }
    }
}
