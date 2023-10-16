namespace ENF_Dist_Test
{
    public class Order {
        public int OrderId {get; set;}
        public Employee Employee { get; set; }
        public Product Product { get; set; }
        public int Quantity {get; set;}
        public Status OrderStatus {get; set;}

        public enum Status{
            Created,
            Started,
            Finished
        }
    }
}