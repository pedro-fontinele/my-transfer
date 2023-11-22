using System;
using Microsoft.AspNetCore.Mvc;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Servico;

namespace TCloudFileSync.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracaoClientController : ControllerBase
    {
        private readonly IConfiguracaoSftpServico _configuracaoSftpServico;

        public ConfiguracaoClientController(IConfiguracaoSftpServico configuracaoSftpServico)
        {
            _configuracaoSftpServico = configuracaoSftpServico;
        }

        [HttpGet]
		public IActionResult Get()
		{
            try
            {
                var retorno = _configuracaoSftpServico.BuscaConfiguracoesSftp();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
		}

        [HttpPut]
        public IActionResult Put([FromBody] ConfiguracaoClienteSftp  configuracaoClienteSftp)
        {
            try
            {
                var retorno = _configuracaoSftpServico.InsereOuAtualizaConfiguracaoSftp(configuracaoClienteSftp);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
