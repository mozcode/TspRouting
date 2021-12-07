using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TspRouting.WebUI.Models
{
    public class CostViewModel
    {
        public CostViewModel(int iterationNumber, double totalCost)
        {
            IterationNumber = iterationNumber;
            TotalCost = totalCost;
        }

        public int IterationNumber { get; set; }

        public double TotalCost { get; set; }
    }
}
