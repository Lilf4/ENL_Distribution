using System.ComponentModel;

namespace ENF_Dist_Test
{
    public class Product{
        public int ProductId {get; set;}
        public string Name {get; set;} = string.Empty;
        public int Quantity {get; set;}
        public string Description {get; set;} = string.Empty;
        public Location Location { get; set; } = new();

        public override string ToString() {
            return Name;
        }
    }
}