using System.Collections.Generic;

namespace TCloudFileSync.Infra.Entidade
{
    public class ConfiguracaoSftp : Entidade
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public ICollection<Rotina> Rotinas { get; set; } 
    }
}
