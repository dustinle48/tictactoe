using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TicTacToe.Server.Services;

public class TwoTokensHandler : AuthorizationHandler<TwoTokensRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        TwoTokensRequirement requirement)
    {
        if (context.Resource is AuthorizationFilterContext filterContext)
        {
            var httpContext = filterContext.HttpContext;

            bool allTokens =
                requirement.TokenNames.All(tokenName => httpContext.Request.Headers.ContainsKey(tokenName));
            
            if (allTokens)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }

        return Task.CompletedTask;
    }
}