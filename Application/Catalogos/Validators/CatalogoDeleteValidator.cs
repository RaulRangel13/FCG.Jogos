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

namespace Application.Catalogos.Validators
{
    public class CatalogoDeleteValidator : ValidatorBase<Catalogo>
    {
        private readonly ICatalogoFind _catalogoFinder;

        public CatalogoDeleteValidator(ICatalogoFind catalogoFinder)
        {
            _catalogoFinder = catalogoFinder;

            RuleFor(x => x)
                .MustAsync(async (x, _) => await _catalogoFinder.FindAnyByFilter(new CatalogoFilter() { id = x.id })).WithMessage("Catalogo não encontrado");

        }
    }
}
