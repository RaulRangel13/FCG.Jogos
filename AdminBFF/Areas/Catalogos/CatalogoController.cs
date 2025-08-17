using Application._Core;
using Application.Catalogos.Queries;
using Microsoft.AspNetCore.Mvc;
using UTIL.Errors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminBFF.Areas.Catalogo
{
    [ApiController]
    [Area("Organizacao")]
    [Route("api/catalogo/[controller]")]
    public class CatalogoController : ControllerBase
    {
        private readonly IQueryHandler<CatalogoFindOneParams, Domain.Catalogos.Entities.Catalogo> _queryOneHandler;

        public CatalogoController(IQueryHandler<CatalogoFindOneParams, Domain.Catalogos.Entities.Catalogo> queryOneHandler)
        {
            _queryOneHandler = queryOneHandler;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var requestParams = new CatalogoFindOneParams { id = id };

            var resultOne = await this._queryOneHandler.handle(requestParams);

            if (resultOne is null)
            {
                return this.NotFound("Não encontrado");
            }

            return Ok(resultOne);
        }

        // GET api/<ValuesController>/5
        [HttpGet]
        public string Get()
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
