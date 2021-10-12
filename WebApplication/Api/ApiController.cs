using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
		private readonly IMemoryCache memoryCache;

		public ApiController(ILogger<ApiController> logger, IConfiguration configuration, IMemoryCache memoryCache)
		{
			this.logger = logger;
			this.configuration = configuration;
			this.memoryCache = memoryCache;
		}

		[HttpGet]
		public IActionResult Main(String json)
		{
			if (memoryCache.TryGetValue<Int32>(json, out var cacheResult) == true)
			{
				logger.Log(LogLevel.Information, "api:main data from cache: {0}", cacheResult);
				return new ObjectResult(cacheResult);
			}

			var limits = new Limit(logger, configuration, "NumberLimits");
			try
			{
				var data = JsonConvert.DeserializeAnonymousType(json, new { values = new List<Int32>() });
				if (data.values.Count > limits.MaxCount)
				{
					logger.Log(LogLevel.Information, "api:main values.length > limits.MaxCount");
					return StatusCode(415, "count size error");
				}

				var result = 0;
				foreach (var value in data.values)
				{
					if (value < limits.MinValue || value > limits.MaxValue)
					{
						logger.Log(LogLevel.Information, "api:main value < limits.MinValue || value > limits.MaxValue");
						return StatusCode(415, "value size error");
					}

					result += (Int32)Math.Pow(value, 2);
				}

				memoryCache.Set(json, result,
					new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
				return new ObjectResult(result);
			}
			catch (Exception exception)
			{
				logger.Log(LogLevel.Error, "api:main exception: {0}", exception.Message);
				return StatusCode(400, "wrong data format");
			}
		}
	}
}