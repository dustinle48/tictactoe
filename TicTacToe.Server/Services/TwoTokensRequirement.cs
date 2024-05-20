using Microsoft.AspNetCore.Authorization;

namespace TicTacToe.Server.Services;

public class TwoTokensRequirement : IAuthorizationRequirement
{
    public IEnumerable<string> TokenNames { get; }

    public TwoTokensRequirement(IEnumerable<string> tokenNames)
    {
        TokenNames = tokenNames;
    }
}