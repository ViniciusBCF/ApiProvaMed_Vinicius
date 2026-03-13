using ApiMedTest.Domain._Helper;
using ApiMedTest.Domain.Entities;
using ApiMedTest.Domain.Entities.Enums;
using FluentAssertions;

namespace ApiMedTest.Tests.Contatos
{
    [Trait("Contato", "Regras de dominio")]
    public class ContatoValidationTests
    {
        [Fact]
        public void AtualizarDados_QuandoDadosValidos_DeveRetornarSemErros()
        {
            var resultado = Contato.Validar(
                "Jose Souza",
                DateTime.Today.AddYears(-25),
                Sexo.Masculino);

            resultado.Should().BeEmpty();
        }

        [Fact]
        public void AtualizarDados_QuandoContatoMenorDeIdade_DeveRetornarErro()
        {
            var resultado = Contato.Validar(
                "Jose Souza",
                DateTime.Today.AddYears(-17),
                Sexo.Masculino);

            resultado.Should().Contain(Constantes.ContatoMenorDeIdadeErrorMsg);
        }

        [Fact]
        public void AtualizarDados_QuandoDataNascimentoFutura_DeveRetornarErro()
        {
            var resultado = Contato.Validar(
                "Jose Souza",
                DateTime.Today.AddDays(1),
                Sexo.Masculino);

            resultado.Should().Contain(Constantes.DataNascimentoInvalidaErrorMsg);
        }

        [Fact]
        public void AtualizarDados_QuandoIdadeZero_DeveRetornarErro()
        {
            var resultado = Contato.Validar(
                "Jose Souza",
                DateTime.Today,
                Sexo.Masculino);

            resultado.Should().Contain(Constantes.IdadeInvalidaErrorMsg);
        }

        [Fact]
        public void AtualizarDados_QuandoNomeMaiorQueLimite_DeveRetornarErro()
        {
            var resultado = Contato.Validar(
                new string('a', Constantes.Numero128 + 1),
                DateTime.Today.AddYears(-20),
                Sexo.Masculino);

            resultado.Should().NotBeEmpty();
        }
    }
}
