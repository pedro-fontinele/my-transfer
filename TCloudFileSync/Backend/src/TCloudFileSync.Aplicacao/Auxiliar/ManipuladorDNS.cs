using System;

namespace TCloudFileSync.Aplicacao.Auxiliar
{
    public static class ManipuladorDns
    {
        /// <summary>
        /// Busca número do IP correspondente ao DNS do host informado 
        /// </summary>
        public static string BuscaEnderecoDoHost(string host)
        {
            try
            {
                var addresses = System.Net.Dns.GetHostAddresses(host);
                if (addresses.Length > 0)
                {
                    return addresses[0].ToString();
                }
                else
                {
                    return "Endereço não encontrado";
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                return "Erro ao buscar o endereço do host: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Erro inesperado: " + ex.Message;
            }
        }
    }
}
