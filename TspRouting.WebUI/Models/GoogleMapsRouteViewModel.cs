using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using TspRouting.WebUI.Entities;

namespace TspRouting.WebUI.Models
{
    public class GoogleMapsRouteViewModel
    {
        public GaEnvironments GaEnvironments { get; set; }

        public IFormFile CoordinateFile { get; set; }

        public List<Node> Nodes { get; set; }

        public Node MapCenter { get; set; }

        public int MapZoom { get; set; }

        public Results Results { get; set; }

        public int NodeCount { get; set; }

        public List<CostViewModel> CostViewModels { get; set; }

        public double[,] DistanceMatrix { get; set; }
    }
}
