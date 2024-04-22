namespace RequestProcessingPipeline
{
	public class FromOneToTenMiddleware
	{
		private readonly RequestDelegate _next;

		public FromOneToTenMiddleware(RequestDelegate next)
		{
			this._next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			string? token = context.Request.Query["number"]; // Получим число из контекста запроса
			try
			{
				int number = Convert.ToInt32(token);
				number = Math.Abs(number);
				if (number == 10)
				{
					// Выдаем окончательный ответ клиенту
					await context.Response.WriteAsync("Your number is ten");
				}
				else
				{
					string[] Ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

				
					if (number > 20)

						context.Session.SetString("number", Ones[number % 10 - 1]);

					else
						await context.Response.WriteAsync("Your number is " + Ones[number - 1]);
				}

			}
		
			catch (Exception)
			{
				// Выдаем окончательный ответ клиенту
				await context.Response.WriteAsync("Incorrect parameter from 1 to 10");
			}
		}
	}
}
