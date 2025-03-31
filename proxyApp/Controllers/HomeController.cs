using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace proxyApp.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetLandingMessage()
        {
            // Basic HTML content
            var html = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Welcome</title>
                </head>
                <body>
                    <h1>Hello</h1>
                    <p>
                        Welcome to the Reverse Proxy Application. 
                        This app forwards requests to a dynamic ngrok endpoint 
                        and provides admin capabilities to update settings on the fly.
                    </p>
                    <p>
                        <strong>Author:</strong> Mohammed Khalil
                    </p>
                </body>
                </html>";

            return new ContentResult
            {
                ContentType = "text/html",
                Content = html,
                StatusCode = 200
            };
        }
    }
}
