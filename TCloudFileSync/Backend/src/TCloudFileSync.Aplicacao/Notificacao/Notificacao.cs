using Newtonsoft.Json;
using System.Collections.Generic;

namespace TCloudFileSync.Aplicacao
{
    public class Notificacao
    {
        private readonly List<string> _notificacoes = new List<string>();

        public bool HouveNotificacao() => _notificacoes.Count != 0;

        public void LimpaNotificacoes() => _notificacoes.Clear();

        public string ExibeNotificacoes() => (_notificacoes.Count != 0) 
            ? JsonConvert.SerializeObject(_notificacoes) 
            : string.Empty;

        public void Adiciona(string mensagem) => _notificacoes.Add(mensagem);
        public void Adiciona(IEnumerable<string> mensagem) => _notificacoes.AddRange(mensagem);
    }
}
