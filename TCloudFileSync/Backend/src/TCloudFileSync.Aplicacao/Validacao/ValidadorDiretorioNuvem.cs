namespace TCloudFileSync.Aplicacao.Validacao
{
    public static class ValidadorDiretorioNuvem
    {
        /// <summary>
        /// Valida se arquivo encontrado na nuvem corresponde às opções de retorno ao diretório anterior. Exemplos: ".", ".."
        /// </summary>
        public static bool ArquivoValido(string nomeArquivo)
        {
            for (var i = 0; i < nomeArquivo.Length; i++)
            {
                if (nomeArquivo[i] != '.')
                {
                    return true;
                }
            }

            return false;
        }
    }
}
