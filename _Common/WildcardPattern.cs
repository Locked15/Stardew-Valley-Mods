﻿using System;
using SRegex = System.Text.RegularExpressions.Regex;

namespace Shockah.CommonModCode
{
	public static class WildcardPatterns
	{
		public static IWildcardPattern Parse(string pattern)
		{
			if (pattern.Contains('*') || pattern.Contains('?') || pattern.Contains('|'))
				return new IWildcardPattern.Impl(pattern);
			return new IWildcardPattern.JustString(pattern);
		}
	}

	public interface IWildcardPattern
	{
		string Pattern { get; }

		bool Matches(string input);

		public readonly struct JustString : IWildcardPattern
		{
			public readonly string Pattern { get; }

			public JustString(string pattern)
			{
				this.Pattern = pattern;
			}

			public bool Matches(string input)
				=> input.Trim().Equals(Pattern.Trim(), StringComparison.InvariantCultureIgnoreCase);
		}

		public class Impl : IWildcardPattern
		{
			public string Pattern { get; private set; }
			private readonly Lazy<SRegex> Regex;

			public Impl(string pattern)
			{
				this.Pattern = pattern;
				Regex = new(() =>
				{
					string regexPattern = "";
					foreach (string part in SRegex.Split(pattern.Trim(), "(\\*+|[\\?\\|])"))
					{
						if (part.Length == 0)
							continue;
						regexPattern += part[0] switch
						{
							'*' => ".*",
							'?' => "\\w+",
							'|' => "\\b",
							_ => SRegex.Escape(part),
						};
					}
					return new SRegex($"^{regexPattern}$");
				});
			}

			public bool Matches(string input)
				=> Regex.Value.IsMatch(input.Trim());
		}
	}
}
