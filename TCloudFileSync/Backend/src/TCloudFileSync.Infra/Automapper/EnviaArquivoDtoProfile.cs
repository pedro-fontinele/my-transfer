using AutoMapper;
using System.Collections.Generic;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Infra.Entidade;

namespace TCloudFileSync.Infra.Automapper
{
    public class EnviaArquivoDtoProfile : Profile
    {
        public EnviaArquivoDtoProfile()
        {
            CreateMap<ConfiguracaoSftp, ConfiguracaoClienteSftp>().ReverseMap();

            CreateMap<Rotina, ConfiguracaoEnvioArquivo>()
                .ForMember(dest => dest.Ativo,                      conf => conf.MapFrom<bool>(x => x.Ativo == "S"))
                .ForMember(dest => dest.DeveApagarArquivoOrigem,    conf => conf.MapFrom<bool>(x => x.Ativo == "S"))
                .ForMember(dest => dest.FluxoLocalParaNuvem,        opt => opt.MapFrom<bool>(o => o.FluxoLocalParaNuvem == "S"))
                .ReverseMap()
                ;

            CreateMap<HistoricoSincronismo, HistoricoSincronismoDto>().ReverseMap();
        }
    }
}
