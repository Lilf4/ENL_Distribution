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
            Regex rx = new("((^\\d{8})|(^\\d{2}[ ]\\d{2}[ ]\\d{2}[ ]\\d{2})|(^\\d{4}[ ]\\d{4}))$");
            return rx.Match(phoneNumber).Success;
        }
    }
}