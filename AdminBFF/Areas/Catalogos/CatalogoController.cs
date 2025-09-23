using AdminBFF.Areas.Catalogos.Payloads;
using AdminBFF.Areas.Catalogos.QueryParams;
using Application._Core;
using Application.Catalogos.Commands;
using Application.Catalogos.Queries;
using Domain.Catalogos.Commands;
using Domain.Catalogos.DTO;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UTIL.Errors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminBFF.Areas.Catalogo
{
    [ApiController]
    [Area("Organizacao")]
    [Route("api/catalogo/")]
    public class CatalogoController : ControllerBase
    {
        private readonly IQueryHandler<CatalogoFindOneParams, Domain.Catalogos.Entities.Catalogo> _queryOneHandler;
        private readonly IQueryHandler<CatalogoFindAllParams, List<CatalogoDTO>> _queryAllHandler;
        private readonly ICommandHandler<CatalogoNewCmd, CatalogoNewCmd.Result> _commandHandler;
        private readonly ICommandHandler<CatalogoChangeCmd, CatalogoChangeCmd.Result> _commandChangeHandler;
        private readonly ICommandHandler<CatalogoDeleteCmd, CatalogoDeleteCmd.Result> _commandDeleteHandler;
        private readonly ICatalogoSearchService _searchService;

        public CatalogoController(
            IQueryHandler<CatalogoFindOneParams, Domain.Catalogos.Entities.Catalogo> queryOneHandler, 
            IQueryHandler<CatalogoFindAllParams, List<CatalogoDTO>> queryAllHandler, 
            ICommandHandler<CatalogoNewCmd, CatalogoNewCmd.Result> commandHandler, 
            ICommandHandler<CatalogoChangeCmd, CatalogoChangeCmd.Result> commandChangeHandler,
            ICommandHandler<CatalogoDeleteCmd, CatalogoDeleteCmd.Result> commandDeleteHandler,
            ICatalogoSearchService searchService)
        {
            _queryOneHandler = queryOneHandler;
            _queryAllHandler = queryAllHandler;
            _commandHandler = commandHandler;
            _commandChangeHandler = commandChangeHandler;
            _commandDeleteHandler = commandDeleteHandler;
            _searchService = searchService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int idUsuario, int id)
        {
            var requestParams = new CatalogoFindOneParams { id = id, idUsuario = idUsuario };

            var resultOne = await this._queryOneHandler.handle(requestParams);

            if (resultOne is null)
            {
                return NotFound("Não encontrado");
            }

            return Ok(resultOne);
        }

        // GET api/<ValuesController>/5
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] CatalogoQueryParams queryParams)
        {
            var requestParams = new CatalogoFindAllParams
            {
                idUsuario = queryParams.idUsuario,
                searchValue = queryParams?.searchValue
            };

            var result = await this._queryAllHandler.handle(requestParams);

            if(result is null)
            {
                return NotFound("Não encontrado");
            } 

            
            return Ok(result);

        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CatalogoPayloadNew infoPayload)
        {
            var cmd = new CatalogoNewCmd();

            cmd.itemCatalogo.name = infoPayload.name;
            cmd.itemCatalogo.description = infoPayload.description;

            var result = await this._commandHandler.handle(cmd);

            if (result.isFail)
            {

                return BadRequest(result.listOfErrors);
            }

            return Ok(result.id);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CatalogoPayloadChange infoPayload)
        {

            var cmd = new CatalogoChangeCmd();

            cmd.itemCatalogo.id = id;
            cmd.itemCatalogo.name = infoPayload.name;
            cmd.itemCatalogo.description = infoPayload.description;

            var result = await _commandChangeHandler.handle(cmd);

            if (result.isFail)
            {

                return BadRequest(result.listOfErrors);
            }

            return Ok(result.id);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var cmd = new CatalogoDeleteCmd();

            cmd.itemCatalogo.id = id;

            var result = await this._commandDeleteHandler.handle(cmd);

            if (result.isFail)
            {

                return BadRequest(result.listOfErrors);
            }

            return this.Ok(result.id);
        }

        /// <summary>
        /// Obtém recomendações de jogos com base no histórico do usuário
        /// </summary>
        /// <param name="idUsuario">ID do usuário para o qual buscar recomendações</param>
        /// <param name="limit">Número máximo de recomendações a retornar (padrão: 5)</param>
        /// <returns>Lista de jogos recomendados</returns>
        [HttpGet("jogosSugestoes")]
        [ProducesResponseType(typeof(List<CatalogoDTO>), 200)]
        [ProducesResponseType(typeof(ErrorItemDetail), 400)]
        public async Task<IActionResult> GetGameRecommendations([FromQuery] int idUsuario, [FromQuery] int limit = 5)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new ErrorItemDetail("IdUsuarioInvalido", "O ID do usuário é inválido", ""));
            }

            try
            {
                var recomendacoes = await _searchService.GetGameRecommendationsAsync(idUsuario, limit);
                var recomendacoesDTO = recomendacoes.Select(r => new CatalogoDTO
                {
                    id = r.id,
                    name = r.name,
                    description = r.description,
                    created_at = r.created_at,
                    alter_at = r.alter_at
                }).ToList();

                return Ok(recomendacoesDTO);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Erro ao buscar recomendações: {ex.Message}");
                return StatusCode(500, new ErrorItemDetail("ErroInterno", "Ocorreu um erro ao processar as recomendações", ""));
            }
        }
    }
}
