using System.Collections.Generic;

namespace homebrewAppServerAPI.Resources
{
    public class WaterProfileResource
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<WaterProfileAdditionResource> Additions { get; set; }
        public string Description { get; set; }
    }
}
