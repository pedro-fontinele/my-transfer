using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Servico;

namespace TCloudFileSync.Api.Controllers.Rotina
{
    [Route("api/[controller]")]
    [ApiController]
    public class RotinaController : ControllerBase
    {
        private readonly IRotinaServico _rotinaServico;

        public RotinaController(IRotinaServico rotinaServico)
        {
            _rotinaServico = rotinaServico;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var retorno = _rotinaServico.BuscaTodasAsRotinas();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("id/{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                var retorno = _rotinaServico.BuscaRotinaPorId(id);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ConfiguracaoEnvioArquivo consultaConfiguracaoEnvioArquivo)
        {
            try
            {
                var retorno = await _rotinaServico.InsereOuAtualizaRotina(consultaConfiguracaoEnvioArquivo);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
