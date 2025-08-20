using Application._Core;
using Domain.Catalogos.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogos.Validators
{
    public class CatalogoNewValidator : ValidatorBase<Catalogo>
    {
        public CatalogoNewValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("O Campo name é obrigatorio");
            
            RuleFor(x => x.description)
                .NotEmpty().WithMessage("O Campo name é description");
        }
    }
}
