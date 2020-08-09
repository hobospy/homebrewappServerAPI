using System;

namespace homebrewAppServerAPI.Resources
{
    public class TastingNoteResource
    {
        public int ID { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public int BrewID { get; set; }
    }
}
