using System;

namespace ENF_Dist_Test
{
    public class Employee{
        public int EmployeeId { get; set; }
        public string FirstName {get; set;} = string.Empty;
        public string LastName {get; set;} = string.Empty;
        public string PhoneNumber {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public JobTitle Title {get; set;} 
        public int CompletedOrders {get; set;}

        public enum JobTitle
        {
            Worker,
            TeamLeader,
            Chief
        }

        public override string ToString() {
            return $"{FirstName} {LastName}";
        }
    }
}