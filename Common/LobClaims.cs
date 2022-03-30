using System.Security.Claims;

namespace Common;

public class LobClaims
{
    public static Claim PremiumUser { get; } = new Claim(ClaimTypes.Role, "premium");
}