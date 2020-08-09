using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace homebrewAppServerAPI.Resources
{
    public class TimerResource
    {
        public int ID { get; set; }

        public long Duration { get; set; }

        public string Type { get; set; }
        public int RecipeStepID { get; set; }
    }
}
