﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Shockah.CommonModCode
{
	public static class ReflectionExtensions
	{
		public static string GetBestName(this Type type)
			=> type.FullName ?? type.Name;

		public static bool IsBuiltInDebugConfiguration(this Assembly assembly)
			=> assembly.GetCustomAttributes(false).OfType<DebuggableAttribute>().Any(attr => attr.IsJITTrackingEnabled);
	}
}
