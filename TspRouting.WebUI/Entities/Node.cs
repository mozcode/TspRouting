namespace TspRouting.WebUI.Entities
{
    public class Node
    {
        public Node(int number, double latitude, double longitude)
        {
            No = number;
            Lat = latitude;
            Lng = longitude;
        }

        public int No { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
