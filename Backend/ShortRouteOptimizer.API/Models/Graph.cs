namespace ShortRouteOptimizer.API.Models
{
    public class Graph
    {
        public Dictionary<string, Node> Nodes { get; set; } = new Dictionary<string, Node>();

        public void AddNode(string nodeName)
        {
            if (!Nodes.ContainsKey(nodeName))
            {
                Nodes[nodeName] = new Node(nodeName);
            }
        }

        public void AddEdge(string from, string to, int weight)
        {
            if (Nodes.ContainsKey(from) && Nodes.ContainsKey(to))
            {
                Nodes[from].AddEdge(Nodes[to], weight);
            }
        }

        public Node? GetNode(string nodeName)
        {
            return Nodes.ContainsKey(nodeName) ? Nodes[nodeName] : null;
        }

        public List<string> GetAllNodeNames()
        {
            return Nodes.Keys.OrderBy(k => k).ToList();
        }
    }
}