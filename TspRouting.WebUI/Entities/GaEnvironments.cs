namespace TspRouting.WebUI.Entities
{
    public class GaEnvironments
    {
        public GaEnvironments()
        {
            PopulationSize = 100;
            Elitism = 2;
            MutationRate = 0.04;
            TwoOptRate = 0.06;
            IterationNumber = 300;
            IterationTerminationPercent = 100;
            RandomIntegerCount = 4;
        }

        public int PopulationSize { get; set; }

        public int Elitism { get; set; }

        public double MutationRate { get; set; }

        public double TwoOptRate { get; set; }

        public int IterationNumber { get; set; }

        public int IterationTerminationPercent { get; set; }

        public int RandomIntegerCount { get; set; }
    }
}