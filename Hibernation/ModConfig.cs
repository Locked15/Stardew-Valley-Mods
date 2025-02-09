﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Shockah.Hibernation
{
	internal class ModConfig
	{
		public bool ModIsEnabled { get; set; } = true;

		public IList<string> LengthOptions { get; set; } = new[] { "3d", "1w", "1s", "1y", "5y", "forever" }.ToList();

		[JsonIgnore]
		public IList<HibernateLength> ParsedLengthOptions
		{
			get
			{
				IList<HibernateLength> parsed = new List<HibernateLength>();
				foreach (var length in LengthOptions)
				{
					var parsedLength = HibernateLength.ParseOrNull(length);
					if (parsedLength is not null)
						parsed.Add(parsedLength.Value);
				}
				return parsed;
			}
		}
	}
}
