using System.Runtime.Intrinsics.X86;

namespace RequestProcessingPipeline
{
	public class FromTwentyToHundredMiddleware
	{
		private readonly RequestDelegate _next;

		public FromTwentyToHundredMiddleware(RequestDelegate next)
		{
			this._next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			string? token = context.Request.Query["number"]; 
			try
			{
				int number = Convert.ToInt32(token);
				number = Math.Abs(number);
				string? result = string.Empty;
				string[] Tens = { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
				if (number < 20)
				{
					await _next.Invoke(context); 
				}
				else if (number > 100)
				{
					number = number % 100;
					if (number < 20)
					{
						await _next.Invoke(context);
						result = context.Session.GetString("number");
						context.Session.SetString("number", result);
					}
					else if (number % 10 == 0)
					{
						context.Session.SetString("number", Tens[number / 10 - 2]);
					}
				
					else

					{
						await _next.Invoke(context);
						result = context.Session.GetString("number");
						context.Session.SetString("number", Tens[number / 10 - 2] + " " + result);
					}
				}
				else
				{
					if (number % 10 == 0)
					{
						await context.Response.WriteAsync("Your number is " + Tens[number / 10 - 2]);
						//context.Session.SetString("number", Tens[number / 10 - 2]);
					}


					else if (number == 100)
					{
						// Выдаем окончательный ответ клиенту
						await context.Response.WriteAsync("Your number is one hundred");
					}
					else
					{
						await _next.Invoke(context);
						result = context.Session.GetString("number");
						await context.Response.WriteAsync("Your number is " + Tens[number / 10 - 2] + " " + result);
					}
				}

			}

			catch (Exception)
			{
				// Выдаем окончательный ответ клиенту
				await context.Response.WriteAsync("Incorrect parameter 20 to 100");
			}
		}
	}
}
