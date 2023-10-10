using System;

namespace ENF_Dist_Test
{
    public class Employee{
        int EmployeeId {get; set;}
        int CompletedOrders {get; set;}
        string FirstName {get; set;}
        string LastName {get; set;}
        string PhoneNumber {get; set;}
        string Email {get; set;}
        JobTitles JobTitle {get; set;} 

        enum JobTitles
        {
            Chief,
            Worker,
            TeamLeader
        }
    }
}