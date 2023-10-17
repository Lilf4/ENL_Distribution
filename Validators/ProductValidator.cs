using FluentValidation;
using System.Text.RegularExpressions;

namespace ENF_Dist_Test.Validators {
    public class ProductValidator : AbstractValidator<Product> {
        public ProductValidator() {
            RuleFor(product => product.Name).NotEmpty();
            RuleFor(product => product.Description).NotEmpty();
            RuleFor(product => product.Quantity).GreaterThanOrEqualTo(0);
            RuleFor(product => product.Location).NotNull();
        }
    }
}