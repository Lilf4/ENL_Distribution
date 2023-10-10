using System;

namespace ENF_Dist_Test
{
    public class Employee{
        int EmployeeId {get; set;}
        int CompletedOrders {get; set;}
        string FirstName {get; set;} = string.Empty;
        string LastName {get; set;} = string.Empty;
        string PhoneNumber {get; set;} = string.Empty;
        string Email {get; set;} = string.Empty;
        JobTitles JobTitle {get; set;} 

        enum JobTitles
        {
            Chief,
            Worker,
            TeamLeader
        }
    }
}