namespace ShortRouteOptimizer.API.Models
{
    public class Edge
    {
        public Node Target { get; set; } = null!;
        public int Weight { get; set; }

        public Edge() { }

        public Edge(Node target, int weight)
        {
            Target = target;
            Weight = weight;
        }
    }
}