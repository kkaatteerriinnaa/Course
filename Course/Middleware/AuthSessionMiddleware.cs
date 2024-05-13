using Course.Data.DAL;
using Course.Middleware;
using System.Globalization;
using System.Security.Claims;

namespace Course.Middleware
{
    public class AuthSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DataAccessor dataAccessor)
        {

            if (context.Request.Query.ContainsKey("logout"))
            {
                context.Session.Remove("auth-user-id");
                context.Response.Redirect("/");
                return;
            }
            if (context.Session.GetString("auth-user-id") is String userId)
            {
                var user = dataAccessor.UserDao.GetUserById(userId);
                if(user != null)
                {
                      Claim[] claim = new Claim[]
                      {
                            new(ClaimTypes.Sid,      userId),
                            new(ClaimTypes.Email,    user.Email),
                            new(ClaimTypes.Name,     user.Name),
                            new(ClaimTypes.UserData, user.AvatarUrl ?? ""),
                            new(ClaimTypes.Role, user.Role ?? ""),
                            new("EmailConfirmCode", user.EmailComfirmCode ?? ""),
                      };

                         context.User = new ClaimsPrincipal(
                            new ClaimsIdentity(
                                claim,
                                nameof(AuthSessionMiddleware)
                            )
                        );
                }

               
                //context.Items.Add("auth", "ok");
            }

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }

    }
}

public static class RequestCultureMiddlewareExtension
{
    public static IApplicationBuilder UseAuthSession(this IApplicationBuilder app) 
    {
        return app.UseMiddleware<AuthSessionMiddleware>();
    }
}
