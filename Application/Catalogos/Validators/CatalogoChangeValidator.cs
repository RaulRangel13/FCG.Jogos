using Application._Core;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Filters;
using Domain.Catalogos.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTIL.Errors;

namespace Application.Catalogos.Validators
{
    public class CatalogoChangeValidator : ValidatorBase<Catalogo>
    {
        private readonly ICatalogoFind _catalogoFinder;
        public CatalogoChangeValidator(ICatalogoFind catalogoFinder) 
        {
            _catalogoFinder = catalogoFinder;

            RuleFor(x => x.id)
                .MustAsync(async (command, id, _) => await _catalogoFinder.FindAnyByFilter(new CatalogoFilter() { id = id }))
                .WithName("id")
                .WithMessage("Catalogo informado não existe.")
                .WithErrorCode(ErrorIssuesConst.ResourceNotFound);
        }
    }
}
