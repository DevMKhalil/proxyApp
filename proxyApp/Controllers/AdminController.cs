using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace proxyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly INgrokUrlService _ngrokUrlService;
        private readonly ILogger<AdminController> _logger;
        private readonly string _secretAdminKey = string.Empty;

        public AdminController(
            INgrokUrlService ngrokUrlService,
            IConfiguration configuration,
            ILogger<AdminController> logger)
        {
            _ngrokUrlService = ngrokUrlService;
            _logger = logger;
            _secretAdminKey = configuration.GetValue<string>("SecretAdminKey");
        }

        // Example request model
        public class UpdateNgrokUrlRequest
        {
            public string NewUrl { get; set; }
        }

        [HttpPost("update-ngrok-url")]
        public IActionResult UpdateNgrokUrl([FromBody] UpdateNgrokUrlRequest request)
        {
            // Check for a simple admin key in the headers
            if (!Request.Headers.TryGetValue("X-ADMIN-KEY", out var adminKey) || adminKey != _secretAdminKey)
            {
                return Unauthorized("Invalid admin key.");
            }

            // For security, add your own validation (e.g., check for an admin API key)
            if (string.IsNullOrEmpty(request.NewUrl))
            {
                return BadRequest("NewUrl is required.");
            }

            _ngrokUrlService.UpdateNgrokUrl(request.NewUrl);
            _logger.LogInformation("NgrokUrl updated to: {NewUrl}", request.NewUrl);
            return Ok(new { message = "NgrokUrl updated successfully", newUrl = _ngrokUrlService.GetNgrokUrl() });
        }
    }

}
