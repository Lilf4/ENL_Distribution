using FluentValidation;

namespace ENF_Dist_Test.Validators {
    public class OrderValidator : AbstractValidator<Order> {
        public OrderValidator() {
            RuleFor(order => order.Quantity).GreaterThanOrEqualTo(1);
            RuleFor(order => order.Employee).NotNull();
            RuleFor(order => order.Product).NotNull();
        }
    }
}