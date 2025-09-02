using Application._Core;
using Domain.Catalogos.Commands;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UTIL.Extensions;

namespace Application.Catalogos.Commands
{
    public class CatalogoNewHandler : ICommandHandler<CatalogoNewCmd, CatalogoNewCmd.Result>
    {
        private readonly ICatalogoCreate _creator;
        private readonly ValidatorBase<Catalogo> _validatorEntity;
        private readonly ICatalogoSearchService _searchService;
        private readonly ILogger<CatalogoNewHandler> _logger;

        public CatalogoNewHandler(
            ICatalogoCreate creator, 
            [FromKeyedServices("new")] ValidatorBase<Catalogo> validatorEntity,
            ICatalogoSearchService searchService,
            ILogger<CatalogoNewHandler> logger)
        {
            _creator = creator;
            _validatorEntity = validatorEntity;
            _searchService = searchService;
            _logger = logger;
        }

        public async Task<CatalogoNewCmd.Result> handle(CatalogoNewCmd command)
        {
            var result = new CatalogoNewCmd.Result();
            this.clearComand(command);

            // Validação
            var validateEntity = await _validatorEntity.ValidateAsync(command.itemCatalogo);
            if (!validateEntity.IsValid)
            {
                result.AddErrors(validateEntity.GetErrorsFromValidation());
                return result;
            }

            try
            {
                // Cria no banco de dados
                var resultCommand = await _creator.Create(command);
                
                if (resultCommand != null)
                {
                    result.id = resultCommand.id;
                    
                    // Indexa no Elasticsearch 
                    try
                    {
                        // Obtém o catálogo completo do banco de dados
                        var catalogo = new Catalogo
                        {
                            id = resultCommand.id,
                            name = command.itemCatalogo.name,
                            description = command.itemCatalogo.description,
                            created_at = command.itemCatalogo.created_at,
                            alter_at = command.itemCatalogo.alter_at
                        };
                        
                        await _searchService.IndexCatalogoAsync(catalogo);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao indexar catálogo no Elasticsearch");
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar catálogo");
                throw;
            }

        }

        private void clearComand(CatalogoNewCmd command)
        {

            command.itemCatalogo = command.itemCatalogo.getTrimStringProperties();
            command.itemCatalogo.created_at = DateTime.Now.toUnspecified();
        }
    }
}
