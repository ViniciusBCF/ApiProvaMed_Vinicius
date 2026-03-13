using ApiMedTest.Domain.Entities;
using ApiMedTest.Domain.Entities.Enums;
using FluentAssertions;

namespace ApiMedTest.Tests.Contatos
{
    [Trait("Contato", nameof(Contato))]
    public class ContatoTests
    {
        [Fact]
        public void AtualizarDados_DeveAlterarTodasAsInformacoes()
        {
            var dataNascimento = DateTime.Today.AddYears(-30);
            var contato = new Contato(
                "Ana Silva",
                dataNascimento,
                Sexo.Feminino);

            contato.NomeContato.Should().Be("Ana Silva");
            contato.DataNascimento.Should().Be(dataNascimento);
            contato.Sexo.Should().Be(Sexo.Feminino);
        }

        [Fact]
        public void Desativar_DeveMarcarComoInativo()
        {
            var contato = new Contato(
                "Ana Silva",
                DateTime.Today.AddYears(-30),
                Sexo.Feminino);

            contato.Desativar();

            contato.StatusContato.Should().Be(StatusContato.Inativo);
        }

        [Fact]
        public void Ativar_DeveMarcarComoAtivo()
        {
            var contato = new Contato(
                "Ana Silva",
                DateTime.Today.AddYears(-30),
                Sexo.Feminino);
            contato.Desativar();

            contato.Ativar();

            contato.StatusContato.Should().Be(StatusContato.Ativo);
        }
    }
}
