using ShortRouteOptimizer.API.Models;

namespace ShortRouteOptimizer.API.Services.Interfaces
{
    public interface IRouteService
    {
        ShortestPathResult CalculateShortestPath(string startNode, string endNode);
        List<string> GetAllNodes();
    }
}