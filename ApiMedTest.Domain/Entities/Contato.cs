using ApiMedTest.Domain._Helper;
using ApiMedTest.Domain.Entities.Base;
using ApiMedTest.Domain.Entities.Enums;

namespace ApiMedTest.Domain.Entities
{
    public class Contato : Entity
    {
        private Contato()
        {
        }

        public Contato(string nomeContato, DateTime dataNascimento, Sexo sexo)
        {
            var erros = ValidarDados(nomeContato, dataNascimento, sexo);
            if (erros.Count != 0)
            {
                throw new ArgumentException(string.Join(" | ", erros));
            }

            NomeContato = nomeContato.Trim();
            DataNascimento = dataNascimento.Date;
            Sexo = sexo;
        }

        public string NomeContato { get; private set; } = string.Empty;
        public DateTime DataNascimento { get; private set; }
        public Sexo Sexo { get; private set; }
        public StatusContato StatusContato { get; private set; } = StatusContato.Ativo;

        public string[] AtualizarDados(
            string nomeContato,
            DateTime dataNascimento,
            Sexo sexo)
        {
            var erros = ValidarDados(nomeContato, dataNascimento, sexo);
            if (erros.Count != 0)
            {
                return erros.ToArray();
            }

            NomeContato = nomeContato.Trim();
            DataNascimento = dataNascimento.Date;
            Sexo = sexo;

            return [];
        }

        public static string[] Validar(string nomeContato, DateTime dataNascimento, Sexo sexo)
        {
            return ValidarDados(nomeContato, dataNascimento, sexo).ToArray();
        }

        public void Desativar()
        {
            StatusContato = StatusContato.Inativo;
        }

        public void Ativar()
        {
            StatusContato = StatusContato.Ativo;
        }

        private static List<string> ValidarDados(
            string nomeContato,
            DateTime dataNascimento,
            Sexo sexo)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(nomeContato))
            {
                erros.Add(Constantes.CampoVazioErrorMsg(nameof(NomeContato)));
            }
            else if (nomeContato.Trim().Length > Constantes.Numero128)
            {
                erros.Add(Constantes.CampoMaiorErrorMsg(nameof(NomeContato), Constantes.Numero128));
            }

            if (dataNascimento == default)
            {
                erros.Add(Constantes.CampoVazioErrorMsg(nameof(DataNascimento)));
            }
            else
            {
                if (dataNascimento.Date > DateTime.Today)
                {
                    erros.Add(Constantes.DataNascimentoInvalidaErrorMsg);
                }
                else
                {
                    var idade = CalcularIdade(dataNascimento.Date, DateTime.Today);
                    if (idade == 0)
                    {
                        erros.Add(Constantes.IdadeInvalidaErrorMsg);
                    }
                    else if (idade < Constantes.MaioridadeMinima)
                    {
                        erros.Add(Constantes.ContatoMenorDeIdadeErrorMsg);
                    }
                }
            }

            if (!Enum.IsDefined(typeof(Sexo), sexo))
            {
                erros.Add(Constantes.CampoVazioErrorMsg(nameof(Sexo)));
            }

            return erros;
        }

        public static int CalcularIdade(DateTime dataNascimento, DateTime? dataReferencia = null)
        {
            var nascimento = dataNascimento.Date;
            var referencia = (dataReferencia ?? DateTime.Today).Date;

            if (nascimento > referencia)
            {
                return 0;
            }

            var idade = referencia.Year - nascimento.Year;

            if (referencia.Month < nascimento.Month
                || (referencia.Month == nascimento.Month && referencia.Day < nascimento.Day))
            {
                idade--;
            }

            return Math.Max(0, idade);
        }
    }
}
