using System.Security.Claims;

namespace EBook.Web.Extensions;
public static class ClaimsPrincipleExtensions
{
    public static string GetUserNameIdentifier(this ClaimsPrincipal user) 
        => user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}

