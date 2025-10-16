namespace ShortRouteOptimizer.API.Models
{
    public class Node
    {
        public string Name { get; set; } = string.Empty;
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public Node() { }

        public Node(string name)
        {
            Name = name;
        }

        public void AddEdge(Node target, int weight)
        {
            Edges.Add(new Edge { Target = target, Weight = weight });
        }

        public override bool Equals(object? obj)
        {
            if (obj is Node other)
                return Name == other.Name;
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}