namespace ENF_Dist_Test
{
    public class Location{
        int Row {get; set;} = 0;
        int Column {get; set;} = 0;
        string LocationId {get => $"{Row}.{Column}";}
    }
}