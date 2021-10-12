using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Api
{
	[ApiController]
	[Route("api/{route}")]
	public class ApiController : ControllerBase
	{
		private readonly ILogger logger;
		private readonly IConfiguration configuration;

		public ApiController(ILogger<ApiController> logger, IConfiguration configuration)
		{
			this.logger = logger;
			this.configuration = configuration;
		}

		[HttpGet]
		public IActionResult Main(String json)
		{
			var limits = new Limit(logger, configuration, "NumberLimits");

			var data = JsonConvert.DeserializeAnonymousType(json, new { values = new List<Int32>() });
			if (data.values.Count > limits.MaxCount)
			{
				logger.Log(LogLevel.Debug, "api:main values.length > limits.MaxCount");
				return StatusCode(415, "count size error");
			}

			var result = 0;
			foreach (var value in data.values)
			{
				if (value < limits.MinValue || value > limits.MaxValue)
				{
					logger.Log(LogLevel.Debug, "api:main value < limits.MinValue || value > limits.MaxValue");
					return StatusCode(415, "value size error");
				}

				result += (Int32)Math.Pow(value, 2);
			}

			return new ObjectResult(result);
		}
	}
}