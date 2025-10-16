using ShortRouteOptimizer.API.Models;
using ShortRouteOptimizer.API.Services.Interfaces;

namespace ShortRouteOptimizer.API.Services
{
    public class DijkstraService : IRouteService
    {
        private readonly GraphService _graphService;

        public DijkstraService(GraphService graphService)
        {
            _graphService = graphService;
        }

        public ShortestPathResult CalculateShortestPath(string startNode, string endNode)
        {
            var graph = _graphService.GetGraph();

            // Validate nodes exist
            if (!graph.Nodes.ContainsKey(startNode) || !graph.Nodes.ContainsKey(endNode))
            {
                return ShortestPathResult.ErrorResult("Invalid start or end node");
            }

            if (startNode == endNode)
            {
                return ShortestPathResult.SuccessResult(new List<string> { startNode }, 0);
            }

            // Dijkstra's algorithm implementation
            var distances = new Dictionary<Node, int>();
            var previous = new Dictionary<Node, Node?>();
            var unvisited = new HashSet<Node>();

            // Initialize distances
            foreach (var node in graph.Nodes.Values)
            {
                distances[node] = int.MaxValue;
                previous[node] = null;
                unvisited.Add(node);
            }

            distances[graph.Nodes[startNode]] = 0;

            while (unvisited.Count > 0)
            {
                // Get node with smallest distance
                var current = unvisited.OrderBy(n => distances[n]).First();

                // If smallest distance is infinity, no path exists
                if (distances[current] == int.MaxValue)
                    break;

                unvisited.Remove(current);

                // Update distances to neighbors
                foreach (var edge in current.Edges)
                {
                    var neighbor = edge.Target;
                    if (!unvisited.Contains(neighbor))
                        continue;

                    var alt = distances[current] + edge.Weight;
                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = current;
                    }
                }
            }

            // Reconstruct path
            var path = ReconstructPath(previous, graph.Nodes[endNode]);
            var totalDistance = distances[graph.Nodes[endNode]];

            if (path.Count == 0 || totalDistance == int.MaxValue)
            {
                return ShortestPathResult.ErrorResult($"No path found from {startNode} to {endNode}");
            }

            return ShortestPathResult.SuccessResult(path, totalDistance);
        }

        private List<string> ReconstructPath(Dictionary<Node, Node?> previous, Node endNode)
        {
            var path = new List<string>();
            var current = endNode;

            while (current != null)
            {
                path.Insert(0, current.Name);
                current = previous[current];
            }

            return path;
        }

        public List<string> GetAllNodes()
        {
            return _graphService.GetAllNodes();
        }
    }
}