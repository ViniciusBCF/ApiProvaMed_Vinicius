namespace ApiMedTest.Domain._Helper
{
    public static class Constantes
    {
        public const string ContatoNaoEncontradoAtivoErrorMsg = "Contato ativo nao encontrado";
        public const string ContatoNaoEncontradoErrorMsg = "Contato nao encontrado";
        public const string ContatoInativoErrorMsg = "Contato encontrado, mas esta inativo";
        public const string ContatoRemovidoMsg = "Contato removido";
        public const string ContatoDesativadoMsg = "Contato desativado";
        public const string ContatoAtivadoMsg = "Contato ativado";
        public const string DataNascimentoInvalidaErrorMsg = "A data de nascimento nao pode ser maior que a data atual";
        public const string IdadeInvalidaErrorMsg = "A idade nao pode ser igual a 0";
        public const string ContatoMenorDeIdadeErrorMsg = "O contato deve ser maior de idade";
        public const int MaioridadeMinima = 18;
        public const int Numero128 = 128;

        public static string RetornandoContatosMsg(int quantidade) =>
            $"Retornando '{quantidade}' contatos ativos";

        public static string CampoVazioErrorMsg(string nomeCampo) =>
            $"O campo '{nomeCampo}' nao deve estar vazio";

        public static string CampoMaiorErrorMsg(string nomeCampo, int tamanhoMaximo) =>
            $"O campo '{nomeCampo}' nao deve passar de {tamanhoMaximo} caracteres";
    }
}
