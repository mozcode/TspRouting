namespace TspRouting.WebUI.Entities
{
    public class Route
    {
        public Route(int[] nodeNumber, double[,] distanceMatrix)
        {
            NodeNumber = nodeNumber;

            TotalCost = CalculateTotalDistance(NodeNumber, distanceMatrix);
        }

        public int[] NodeNumber { get; set; }

        public double TotalCost { get; set; }

        public double CalculateTotalDistance(int[] nodes, double[,] distanceMatrix)
        {
            double totalDistance = 0;

            int i = 0;

            for (; i < nodes.Length - 1; i++)
            {
                totalDistance += distanceMatrix[nodes[i], nodes[i + 1]];
            }

            //Add first and last node distance to complete circle
            totalDistance += distanceMatrix[nodes[i], nodes[0]];

            return totalDistance;
        }
    }
}