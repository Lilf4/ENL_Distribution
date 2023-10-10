namespace ENF_Dist_Test
{
    public class Order{
        int OrderId {get; set;}
        int EmployeeId {get; set;}
        int Count {get; set;}
        Status OrderStatus {get; set;}

        enum Status{
            Created,
            Started,
            Finished
        }
    }
}