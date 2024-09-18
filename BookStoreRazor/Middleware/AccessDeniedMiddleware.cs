using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
namespace BookStoreRazor.Middleware
{
	public class AccessDeniedMiddleware
	{
		private readonly RequestDelegate _next;

		public AccessDeniedMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			await _next(context);

			if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
			{
				context.Response.Redirect("/Account/AccessDenited");
			}
		}
	}
}
