using Microsoft.AspNetCore.Mvc;
using ShortRouteOptimizer.API.Models;
using ShortRouteOptimizer.API.Services.Interfaces;

namespace ShortRouteOptimizer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;
        private readonly ILogger<RouteController> _logger;

        public RouteController(IRouteService routeService, ILogger<RouteController> logger)
        {
            _routeService = routeService;
            _logger = logger;
        }

        /// <summary>
        /// Calculates the shortest path between two nodes
        /// </summary>
        /// <param name="start">Start node (A-I)</param>
        /// <param name="end">End node (A-I)</param>
        /// <returns>Shortest path result with path and total distance</returns>
        [HttpGet("shortest-path")]
        [ProducesResponseType(typeof(ApiResponse<ShortestPathResult>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        public IActionResult CalculateShortestPath([FromQuery] string start, [FromQuery] string end)
        {
            try
            {
                _logger.LogInformation("Received request to calculate shortest path from {Start} to {End}", start, end);

                // Validate input parameters
                if (string.IsNullOrWhiteSpace(start) || string.IsNullOrWhiteSpace(end))
                {
                    var error = "Both 'start' and 'end' parameters are required";
                    _logger.LogWarning(error);
                    return BadRequest(ApiResponse<object>.ErrorResponse(error, "Missing parameters"));
                }

                start = start.Trim().ToUpper();
                end = end.Trim().ToUpper();

                // Validate node format
                if (!IsValidNode(start) || !IsValidNode(end))
                {
                    var error = "Nodes must be single letters from A to I";
                    _logger.LogWarning("Invalid node format: Start={Start}, End={End}", start, end);
                    return BadRequest(ApiResponse<object>.ErrorResponse(error, "Invalid node format"));
                }

                // Calculate shortest path
                var result = _routeService.CalculateShortestPath(start, end);

                if (!result.Success)
                {
                    _logger.LogWarning("No path found from {Start} to {End}: {Error}", start, end, result.ErrorMessage);
                    return Ok(ApiResponse<ShortestPathResult>.SuccessResponse(result, "No path found"));
                }

                _logger.LogInformation("Successfully calculated path from {Start} to {End}: {Path} (Distance: {Distance})", 
                    start, end, string.Join(" → ", result.ShortestPath), result.TotalDistance);

                return Ok(ApiResponse<ShortestPathResult>.SuccessResponse(result, "Shortest path calculated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating shortest path from {Start} to {End}", start, end);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An internal server error occurred while calculating the route", 
                    "Internal server error"));
            }
        }

        /// <summary>
        /// Gets all available nodes in the graph
        /// </summary>
        /// <returns>List of all nodes (A-I)</returns>
        [HttpGet("nodes")]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), 200)]
        public IActionResult GetAllNodes()
        {
            try
            {
                var nodes = _routeService.GetAllNodes();
                _logger.LogInformation("Retrieved {Count} available nodes", nodes.Count);
                return Ok(ApiResponse<List<string>>.SuccessResponse(nodes, "Nodes retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving nodes");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving nodes", 
                    "Internal server error"));
            }
        }

        /// <summary>
        /// Health check endpoint
        /// </summary>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "API is running", timestamp = DateTime.UtcNow });
        }

        private bool IsValidNode(string node)
        {
            return node.Length == 1 && node[0] >= 'A' && node[0] <= 'I';
        }
    }
}