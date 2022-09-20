﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Shockah.FlexibleSprinklers
{
	public interface ILineSprinklersApi
	{
		/// <summary>Get the relative tile coverage by supported sprinkler ID. Note that sprinkler IDs may change after a save is loaded due to Json Assets reallocating IDs.</summary>
		IDictionary<int, Vector2[]> GetSprinklerCoverage();
	}
}