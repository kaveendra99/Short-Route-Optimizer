using ShortRouteOptimizer.API.Models;
using ShortRouteOptimizer.API.Services.Interfaces;

namespace ShortRouteOptimizer.API.Services
{
    public class GraphService : IRouteService
    {
        private readonly Graph _graph;

        public GraphService()
        {
            _graph = InitializeGraph();
        }

        private Graph InitializeGraph()
        {
            var graph = new Graph();

            // Add all nodes A-I
            var nodes = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I" };
            foreach (var node in nodes)
            {
                graph.AddNode(node);
            }

            // Add directional edges with weights as per specification
            graph.AddEdge("A", "B", 4);
            graph.AddEdge("A", "C", 6);
            graph.AddEdge("C", "D", 8);
            graph.AddEdge("D", "E", 4);
            graph.AddEdge("D", "G", 1);
            graph.AddEdge("E", "F", 3);
            graph.AddEdge("E", "B", 2); // Directional: E → B
            graph.AddEdge("E", "H", 5);
            graph.AddEdge("F", "B", 2);
            graph.AddEdge("F", "H", 6);
            graph.AddEdge("G", "I", 5);
            graph.AddEdge("G", "E", 5);
            graph.AddEdge("H", "I", 8);

            return graph;
        }

        public Graph GetGraph()
        {
            return _graph;
        }

        public List<string> GetAllNodes()
        {
            return _graph.GetAllNodeNames();
        }

        public ShortestPathResult CalculateShortestPath(string startNode, string endNode)
        {
            // This method will be implemented in DijkstraService
            // For now, return empty result
            return ShortestPathResult.ErrorResult("Dijkstra algorithm not implemented yet");
        }
    }
}