namespace homebrewAppServerAPI.Resources
{
    public class IngredientResource
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string Unit { get; set; }
        public int RecipeStepID { get; set; }
    }
}
