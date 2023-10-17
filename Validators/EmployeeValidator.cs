using FluentValidation;
using System.Text.RegularExpressions;

namespace ENF_Dist_Test.Validators {
    public class EmployeeValidator : AbstractValidator<Employee> {
        public EmployeeValidator() {
            RuleFor(employee => employee.FirstName).NotEmpty();
            RuleFor(employee => employee.LastName).NotEmpty();
            RuleFor(employee => employee.PhoneNumber).Must(BeAValidPhoneNumber).WithMessage("Please specify a valid danish phoneNumber");
            RuleFor(employee => employee.Email).EmailAddress();
        }

        private bool BeAValidPhoneNumber(string phoneNumber) {
            Regex rx = new("^(?:(?:00|\\+)?45)?(?=2|3[01]|4[012]|4911|5[0-3]|6[01]|[78]1|9[123])\\d{8}$");
            return rx.Match(phoneNumber).Success;
        }
    }
}