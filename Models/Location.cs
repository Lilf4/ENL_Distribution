namespace ENF_Dist_Test
{
    public class Location{
        public int Row {get; set;} = 0;
        public int Column {get; set;} = 0;
        public string LocationId {get => $"{Row}.{Column}";}
    }
}