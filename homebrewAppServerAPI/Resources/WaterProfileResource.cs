namespace homebrewAppServerAPI.Resources
{
    public class WaterProfileResource
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double LacticAcid { get; set;}
        public double Gypsum { get; set; }
        public double CalciumChloride { get; set; }
        public double EpsomSalt { get; set; }
        public double NonIodizedSalt { get; set; }
        public double BakingSoda { get; set; }
    }
}
