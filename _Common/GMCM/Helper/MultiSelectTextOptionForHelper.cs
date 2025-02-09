﻿using System;
using System.Collections.Generic;

namespace Shockah.CommonModCode.GMCM
{
	public static class MultiSelectTextOptionForHelper
	{
		public static void AddMultiSelectTextOption<T>(
			this GMCMI18nHelper helper,
			string keyPrefix,
			Func<T, bool> getValue,
			Action<T> addValue,
			Action<T> removeValue,
			Func<float, int> columns,
			T[] allowedValues,
			Func<T, string>? formatAllowedValue = null,
			Action? afterValuesUpdated = null,
			object? tokens = null
		)
		{
			helper.Api.AddMultiSelectTextOption(
				mod: helper.Mod,
				name: () => helper.Translations.Get($"{keyPrefix}.name", tokens),
				tooltip: helper.GetOptionalTranslatedStringDelegate($"{keyPrefix}.tooltip", tokens),
				getValue: getValue,
				addValue: addValue,
				removeValue: removeValue,
				columns: columns,
				allowedValues: allowedValues,
				formatAllowedValue: formatAllowedValue,
				afterValuesUpdated: afterValuesUpdated
			);
		}

		public static void AddMultiSelectTextOption<T>(
			this GMCMI18nHelper helper,
			string keyPrefix,
			Func<IReadOnlySet<T>> getValues,
			Action<IReadOnlySet<T>> setValues,
			Func<float, int> columns,
			T[] allowedValues,
			Func<T, string>? formatAllowedValue = null,
			object? tokens = null
		)
		{
			helper.Api.AddMultiSelectTextOption(
				mod: helper.Mod,
				name: () => helper.Translations.Get($"{keyPrefix}.name", tokens),
				tooltip: helper.GetOptionalTranslatedStringDelegate($"{keyPrefix}.tooltip", tokens),
				getValues: getValues,
				setValues: setValues,
				columns: columns,
				allowedValues: allowedValues,
				formatAllowedValue: formatAllowedValue
			);
		}
	}
}
