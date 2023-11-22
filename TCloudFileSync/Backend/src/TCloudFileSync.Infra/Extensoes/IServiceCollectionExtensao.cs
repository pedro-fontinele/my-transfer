using Microsoft.Extensions.DependencyInjection;
using TCloudFileSync.Infra.Automapper;
using TCloudFileSync.Infra.Configuracoes;
using TCloudFileSync.Infra.Contexto;

namespace TCloudFileSync.Infra.Extensoes
{
    public static class IServiceCollectionExtensao
    {
        public static void ConfiguraBaseDeDados(this IServiceCollection services)
        {
            services.AddDbContext<SqliteContexto>();
            services.AddScoped<ContextoConfiguracao>();
            services.AddAutoMapper(typeof (EnviaArquivoDtoProfile));
        }
    }
}
