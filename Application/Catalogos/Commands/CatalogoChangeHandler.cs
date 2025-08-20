using Application._Core;
using Domain.Catalogos.Commands;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTIL.Extensions;

namespace Application.Catalogos.Commands
{
    public class CatalogoChangeHandler : ICommandHandler<CatalogoChangeCmd, CatalogoChangeCmd.Result>
    {
        private readonly ICatalogoChange _change;
        private readonly ValidatorBase<Catalogo> _validatorEntity;

        public CatalogoChangeHandler(ICatalogoChange change, [FromKeyedServices("change")] ValidatorBase<Catalogo> validatorEntity)
        {
            _change = change;
            _validatorEntity = validatorEntity;
        }
        public async Task<CatalogoChangeCmd.Result> handle(CatalogoChangeCmd command)
        {
            var result = new CatalogoChangeCmd.Result();

            this.clearComand(command);

            //Validar
            var validateEntity = await _validatorEntity.ValidateAsync(command.itemCatalogo);

            if (!validateEntity.IsValid)
            {
                result.AddErrors(validateEntity.GetErrorsFromValidation());
                return result;
            }

            //Acionar changer
            await this._change.Change(command);

            result.id = command.itemCatalogo.id;

            return result;
        }

        private void clearComand(CatalogoChangeCmd command)
        {

            command.itemCatalogo = command.itemCatalogo.getTrimStringProperties();
            command.itemCatalogo.alter_at = DateTimeExtensions.nowUnspecified();

        }
    }
}
