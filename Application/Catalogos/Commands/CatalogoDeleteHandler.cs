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

namespace Application.Catalogos.Commands
{
    public class CatalogoDeleteHandler : ICommandHandler<CatalogoDeleteCmd, CatalogoDeleteCmd.Result>
    {
        private readonly ICatalogoDelete _deleter;
        private readonly ValidatorBase<Catalogo> _validatorEntity;

        public CatalogoDeleteHandler(ICatalogoDelete deleter, [FromKeyedServices("delete")] ValidatorBase<Catalogo> validatorEntity)
        {
            _deleter = deleter;
            _validatorEntity = validatorEntity;
        }

        public async Task<CatalogoDeleteCmd.Result> handle(CatalogoDeleteCmd command)
        {
            var result = new CatalogoDeleteCmd.Result();

            var validateEntity = await _validatorEntity.ValidateAsync(command.itemCatalogo);

            if (!validateEntity.IsValid)
            {

                result.AddErrors(validateEntity.GetErrorsFromValidation());

                return result;
            }

            await this._deleter.Delete(command.itemCatalogo.id);


            result.id = command.itemCatalogo.id;

            return result;
        }
    }
}
