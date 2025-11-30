using System.Security.Claims;

namespace Core.Abstractions.Services;

public interface IHttpContextService<TIdType>
    where TIdType : struct, IEquatable<TIdType>
{
    TIdType? GetCurrentUserId();
    List<Claim> GetClaims();
    ClaimsPrincipal? GetCurrentUser();
}
