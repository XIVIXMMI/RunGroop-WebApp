using System.Security.Claims;
namespace RunGroopWebApp
{
	public static class ClaimPricipalExtensions
	{
		public static string GetUserId(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.NameIdentifier).Value;

		}
	}
}



