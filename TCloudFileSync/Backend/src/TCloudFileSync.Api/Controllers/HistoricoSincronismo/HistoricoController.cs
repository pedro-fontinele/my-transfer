using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Servico;

namespace TCloudFileSync.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricoController : ControllerBase
    {
        private readonly IHistoricoSincronismoService _historicoSincronismoService;

        public HistoricoController(IHistoricoSincronismoService historicoSincronismoService)
        {
            _historicoSincronismoService = historicoSincronismoService;
        }

        /// <summary>
        /// Obtém histórico de sincronismo com a opção de carregar mais resultados e definir a quantidade de novas linhas.
        /// </summary>
        /// <param name="carregaMaisResultados">Indica se deve carregar mais resultados.</param>
        /// <returns>Os dados do histórico de sincronismo.</returns>
        [HttpGet("pagina/{pagina}/itensPorPagina/{itensPorPagina}")]
        public async Task<IActionResult> Get([FromRoute] int pagina = 1, [FromRoute] int itensPorPagina = 20)
        {
            try
            {
                var retorno = await _historicoSincronismoService.BuscaHistoricoSincronismo(pagina, itensPorPagina);
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
