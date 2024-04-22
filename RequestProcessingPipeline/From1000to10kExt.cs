namespace RequestProcessingPipeline
{
	public static class From1000to10kExt
	{
		public static IApplicationBuilder UseFrom1000to10k(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<From1000to10kMiddle>();
		}
	}
}
