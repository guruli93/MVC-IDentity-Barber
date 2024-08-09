

public class SaveLastVisitedPageMiddlewareX
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SaveLastVisitedPageMiddlewareX> _logger;

    public SaveLastVisitedPageMiddlewareX(RequestDelegate next, 
        ILogger<SaveLastVisitedPageMiddlewareX> logger)
    {
        _next = next;
        _logger = logger;
    }
    //-------------------------------------------------------------------//
    public async Task InvokeAsync(HttpContext context)
    {
        var queryParameters = context.Request.Query;
       
        var requestPath = context.Request.Path.Value;
        var method = context.Request.Method.ToUpperInvariant();
       
        var fromButton = queryParameters["fromButton"].ToString();
        var excludedPaths2 = new[]
 {
            "/Product/DisplayImage",
            "/Product/Delete",
            "/Booking/GetBookings",
            "/Product/Edit",
            "/Product/Search",
            "/Services/Product",
           
            
        };

        if (excludedPaths2.Any(path => requestPath.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }
   
        else
        {

            if (method == "GET" && !string.IsNullOrEmpty(requestPath))
            {
                if (!string.IsNullOrEmpty(fromButton))
                {
                    await HandleRequestRouteValues(context);
                    return;
                }

                var excludedPaths = new[]
            {
                "/Services/ShowRezervation",
                "/Login/Login",
                "/AccountRegister/Register",
                "/Product/ProductX",
                "/Product/Create"

            };


                if (!Array.Exists(excludedPaths, path => requestPath.Equals(path, StringComparison.OrdinalIgnoreCase)))

                {
                    var lastVisitedPageCookie = context.Request.Cookies["Last"];
                    var lastVisitedPageSession = context.Session.GetString("LastVisitedPage");

                    if (IsValidRedirect(lastVisitedPageCookie, requestPath))
                    {

                        _logger.LogInformation("RedirectToLastVisitedPage lastVisitedPageCookie to session: {lastVisitedPageCookie}", lastVisitedPageCookie);

                        await RedirectToLastVisitedPage(context, lastVisitedPageCookie);
                        return;
                    }

                    if (IsValidRedirect(lastVisitedPageSession, requestPath))
                    {
                        _logger.LogInformation("RedirectToLastVisitedPage lastVisitedPageSession to session: {lastVisitedPageSession}", lastVisitedPageSession);
                        await RedirectToLastVisitedPage(context, lastVisitedPageSession);
                        return;
                    }
                }

                // Save the current path to cookie and session if it's one of the tracked paths
                if (requestPath.Contains("/Account/Register", StringComparison.OrdinalIgnoreCase) ||
                    requestPath.Contains("/Login/Login", StringComparison.OrdinalIgnoreCase) ||
                    requestPath.Contains("/Services/ShowRezervation", StringComparison.OrdinalIgnoreCase))
                {
                    var lastVisitedPageCookie = context.Request.Cookies["Last"];
                    var lastVisitedPageSession = context.Session.GetString("LastVisitedPage");
                    if (requestPath != lastVisitedPageCookie)
                    {
                        context.Response.Cookies.Append("Last", requestPath, new CookieOptions
                        {
                            Secure = true,
                            HttpOnly = true,
                            IsEssential = true,
                            Expires = DateTimeOffset.UtcNow.AddHours(1)
                        });

                        context.Session.SetString("LastVisitedPage", requestPath);

                        _logger.LogInformation("Saved lastVisitedPageCookie to session: {lastVisitedPageCookie}", requestPath);
                        _logger.LogInformation("Saved LastVisitedPage to session: {LastVisitedPage}", requestPath);

                        if (string.IsNullOrEmpty(lastVisitedPageCookie))
                        {
                            await RedirectToLastVisitedPage(context, requestPath);
                            return;
                        }
                    }

                }
            }

            await _next(context);
        }
    }

    //---------------------------------------------------------------------//
    private bool IsValidRedirect(string storedPath, string currentPath)
    {
        return storedPath != currentPath && !string.IsNullOrEmpty(storedPath) && storedPath != "/" && !currentPath.Equals(storedPath,
            StringComparison.OrdinalIgnoreCase);
    }
    //------------------------------------------------------------------------//
    private async Task HandleRequestRouteValues(HttpContext context)
    {
        var routeValues = context.Request.RouteValues;
        var redirectUrl = "/";
        var requestPath = context.Request.Path.Value;
        if (routeValues.ContainsKey("action") && routeValues.ContainsKey("controller"))
        {
            var action = routeValues["action"].ToString();
            var controller = routeValues["controller"].ToString();
            redirectUrl = $"/{controller}/{action}";
            if (action == "Index" && controller == "Home")
            {
                await RedirectToLastVisitedPage(context, redirectUrl);
                return;
            }
        }
       


    }
    //-------------------------------------------------------------------//
    private async Task RedirectToLastVisitedPage(HttpContext context, string redirectPath)
    {
        if (redirectPath.Contains("Home/Index"))
        {
            context.Session.Remove("LastVisitedPage");
            context.Response.Cookies.Delete("Last");
            context.Response.Redirect(redirectPath);
        }
        else
        {

            context.Response.Redirect(redirectPath);
        }

      
        await Task.CompletedTask; 
    }
    //--------------------------------------------------------------------------------------------//
}






