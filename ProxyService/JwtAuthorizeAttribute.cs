using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProxyApi.Attributes.Auth;

/// <summary>
/// Атрибут авторизации пользователя.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UserAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    /// <summary>
    /// Проверка авторизации пользователя.
    /// </summary>
    /// <param name="context"></param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.Items["UserId"];
        if (userId == null)
        {
            context.Result = new JsonResult(new { message = "Пользователь не авторизирован!" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}