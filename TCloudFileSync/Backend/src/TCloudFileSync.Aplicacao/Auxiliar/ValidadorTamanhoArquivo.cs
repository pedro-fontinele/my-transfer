using System.IO;

namespace TCloudFileSync.Aplicacao.Auxiliar
{
    public static class ValidadorTamanhoArquivo
    {
        public static double ValidadorTamanho(string caminhoLocal) 
        {
            if (!File.Exists(caminhoLocal))
            {
                return 0;
            }

            FileInfo fileInfo = new FileInfo(caminhoLocal);

            long tamanhoEmBytes = fileInfo.Length;

            return (double)tamanhoEmBytes / 1024;
        }
    }
}
