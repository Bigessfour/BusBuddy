using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BusBuddy.Controllers
{
    /// <summary>
    /// Controller for health checks to support Docker health monitoring
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthController"/> class.
        /// </summary>
        /// <param name="logger">Logger</param>
        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Simple health check endpoint for Docker container health monitoring
        /// </summary>
        /// <returns>OK status if the application is running</returns>
        [HttpGet]
        [Route("/health")]
        public IActionResult CheckHealth()
        {
            _logger.LogInformation("Health check performed");
            return Ok(new { status = "healthy", timestamp = System.DateTime.UtcNow });
        }
    }
}
