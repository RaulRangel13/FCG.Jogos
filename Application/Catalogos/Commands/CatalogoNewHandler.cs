using Application._Core;
using Domain.Catalogos.Commands;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTIL.Extensions;
using Microsoft.Extensions.DependencyInjection;


namespace Application.Catalogos.Commands
{
    public class CatalogoNewHandler : ICommandHandler<CatalogoNewCmd, CatalogoNewCmd.Result>
    {
        private readonly ICatalogoCreate _creator;
        private readonly ValidatorBase<Catalogo> _validatorEntity;

        public CatalogoNewHandler(ICatalogoCreate creator, [FromKeyedServices("new")] ValidatorBase<Catalogo> validatorEntity)
        {
            _creator = creator;
            _validatorEntity = validatorEntity;
        }

        public async Task<CatalogoNewCmd.Result> handle(CatalogoNewCmd command)
        {

            var result = new CatalogoNewCmd.Result();

            this.clearComand(command);

            //Validar
            var validateEntity = await _validatorEntity.ValidateAsync(command.itemCatalogo);

            if (!validateEntity.IsValid)
            {
                result.AddErrors(validateEntity.GetErrorsFromValidation());
                return result;
            }

            //Fill (se precisar)

            //Acionar creator
            var resultCommand = await this._creator.Create(command);

            result.id = resultCommand.id;

            return result;

        }

        private void clearComand(CatalogoNewCmd command)
        {

            command.itemCatalogo = command.itemCatalogo.getTrimStringProperties();
            command.itemCatalogo.created_at = DateTime.Now.toUnspecified();
        }
    }
}
