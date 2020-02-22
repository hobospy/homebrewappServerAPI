namespace homebrewAppServerAPI.Resources
{
    public class BrewResource
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double ABV { get; set; }
        public RecipeResource Recipe { get; set; }
    }
}
