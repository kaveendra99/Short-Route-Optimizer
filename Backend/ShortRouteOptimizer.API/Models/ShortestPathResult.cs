namespace ShortRouteOptimizer.API.Models
{
    public class ShortestPathResult
    {
        public bool Success { get; set; }
        public List<string>? ShortestPath { get; set; }
        public int TotalDistance { get; set; }
        public string? ErrorMessage { get; set; }

        public static ShortestPathResult SuccessResult(List<string> path, int distance)
        {
            return new ShortestPathResult
            {
                Success = true,
                ShortestPath = path,
                TotalDistance = distance,
                ErrorMessage = null
            };
        }

        public static ShortestPathResult ErrorResult(string errorMessage)
        {
            return new ShortestPathResult
            {
                Success = false,
                ShortestPath = null,
                TotalDistance = 0,
                ErrorMessage = errorMessage
            };
        }
    }
}