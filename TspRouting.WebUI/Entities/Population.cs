using System.Collections.Generic;
using System.Linq;

namespace TspRouting.WebUI.Entities
{
    public class Population
    {
        public Population(List<Route> routes)
        {
            Routes = routes;
            MinimumCost = routes.Min(x => x.TotalCost);
        }
        public List<Route> Routes { get; set; }
        public double MinimumCost { get; set; }
    }
}
