using FluentValidation;
using NLayer.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Validations
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator() 
        {
            RuleFor(x =>  x.Name).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            //RuleFor(x => x.Price).GreaterThan(0).WithMessage("{PropertyName} should be entered");
            RuleFor(x => x.Price).InclusiveBetween(1,decimal.MaxValue).WithMessage("{PropertyName} should be valid");
            RuleFor(x => x.Stock).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} should be valid");
            RuleFor(x => x.CategoryId).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} should be valid"); // gönderilmezse fk hatası 
        }    
    }
}
