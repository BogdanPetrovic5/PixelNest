namespace PixelNestBackend.Middleware
{
    public class APICallLimiter
    {
        private readonly RequestDelegate _next;
        private static readonly Dictionary<string, DateTime> _lastRequestTimes = new();
        private readonly List<string> _rateLimitedEndpoints;
        public APICallLimiter(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _rateLimitedEndpoints = new List<string>
            {
                "/api/Authentication/Register",
                "/api/Post/PublishPost",
                "/api/Post/LikePost",
                "/api/Post/SavePost"

            };
        }
        public async Task Invoke(HttpContext context)
        {
            var requestPath = context.Request.Path.Value;

            
            if (_rateLimitedEndpoints.Contains(requestPath))
            {
                var userIdentifier = context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString();
                if (userIdentifier != null)
                {
                    if (_lastRequestTimes.ContainsKey(userIdentifier))
                    {
                        var lastRequestTime = _lastRequestTimes[userIdentifier];
                        if (DateTime.UtcNow - lastRequestTime < TimeSpan.FromSeconds(1))
                        {
                            context.Response.StatusCode = 429; 
                            await context.Response.WriteAsync("Too many requests. Please wait.");
                            return;
                        }
                    }
                    _lastRequestTimes[userIdentifier] = DateTime.UtcNow;
                }
            }

            await _next(context);
        }
    }
}
