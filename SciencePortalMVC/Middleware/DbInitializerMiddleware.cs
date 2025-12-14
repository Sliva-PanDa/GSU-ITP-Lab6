using SciencePortalMVC.Data;

namespace SciencePortalMVC.Middleware
{
	public class DbInitializerMiddleware
	{
		private readonly RequestDelegate _next;
		public DbInitializerMiddleware(RequestDelegate next) => _next = next;

		public async Task InvokeAsync(HttpContext context, SciencePortalDbContext dbContext)
		{
			if (!context.Session.Keys.Contains("db_initialized"))
			{
				DbInitializer.Initialize(dbContext);
				context.Session.SetString("db_initialized", "true");
			}
			await _next(context);
		}
	}

	public static class DbInitializerExtensions
	{
		public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<DbInitializerMiddleware>();
		}
	}
}