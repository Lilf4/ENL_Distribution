namespace ENF_Dist_Test
{
    public class Product{
        public int ProductId {get; set;}
        public string Name {get; set;} = string.Empty;
        public int Quantity {get; set;}
        public string Description {get; set;} = string.Empty;
        public Location Location { get; set; }

        public override string ToString() {
            return $"{Name}";
        }
        public string NameQuanty {
            get { return $"{Name} ({Quantity})"; }
        }
    }
}