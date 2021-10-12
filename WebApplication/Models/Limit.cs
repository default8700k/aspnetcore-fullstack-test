using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace WebApplication.Models
{
	public class Limit
	{
		public Int32 MinValue { get; private set; }
		public Int32 MaxValue { get; private set; }
		public Int32 MaxCount { get; private set; }

		public Limit(ILogger logger, IConfiguration configuration, String sectionName)
		{
			var section = configuration.GetSection(sectionName);
			try
			{
				MinValue = Int32.Parse(section["MinValue"]);
				MaxValue = Int32.Parse(section["MaxValue"]);
				MaxCount = Int32.Parse(section["MaxCount"]);
			}
			catch (Exception exception)
			{
				logger.Log(LogLevel.Error, "{0}", exception.Message);
				throw new Exception(exception.Message);
			}
		}
	}
}