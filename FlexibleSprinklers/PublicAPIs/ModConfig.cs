﻿using Newtonsoft.Json;
using Shockah.CommonModCode;
using StardewModdingAPI;

namespace Shockah.FlexibleSprinklers
{
	public class ModConfig : IVersioned.Modifiable
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ISemanticVersion? Version { get; set; }
		[JsonProperty] public SprinklerBehaviorEnum SprinklerBehavior { get; internal set; } = SprinklerBehaviorEnum.ClusterWithoutVanilla;
		[JsonProperty] public bool IgnoreRange { get; internal set; } = false;
		[JsonProperty] public FlexibleSprinklerBehaviorTileWaterBalanceMode TileWaterBalanceMode { get; internal set; } = FlexibleSprinklerBehaviorTileWaterBalanceMode.Relaxed;
		[JsonProperty] public bool SplitDisconnectedClusters { get; internal set; } = true;
		[JsonProperty] public ClusterSprinklerBehaviorClusterOrdering ClusterBehaviorClusterOrdering { get; internal set; } = ClusterSprinklerBehaviorClusterOrdering.BiggerFirst;
		[JsonProperty] public ClusterSprinklerBehaviorBetweenClusterBalanceMode ClusterBehaviorBetweenClusterBalanceMode { get; internal set; } = ClusterSprinklerBehaviorBetweenClusterBalanceMode.Relaxed;
		[JsonProperty] public ClusterSprinklerBehaviorInClusterBalanceMode ClusterBehaviorInClusterBalanceMode { get; internal set; } = ClusterSprinklerBehaviorInClusterBalanceMode.Relaxed;
		[JsonProperty] public bool ActivateOnPlacement { get; internal set; } = false;
		[JsonProperty] public bool ActivateOnAction { get; internal set; } = false;
		[JsonProperty] public bool ActivateBeforeSleep { get; internal set; } = true;
		[JsonProperty] public float CoverageAlpha { get; internal set; } = 1;
		[JsonProperty] public float CoverageTimeInSeconds { get; internal set; } = 5;
		[JsonProperty] public float CoverageAnimationInSeconds { get; internal set; } = 1;
		[JsonProperty] public bool CoverageOverlayDuplicates { get; internal set; } = true;
		[JsonProperty] public bool ShowCoverageOnPlacement { get; internal set; } = true;
		[JsonProperty] public bool ShowCoverageOnAction { get; internal set; } = true;
		[JsonProperty] public int Tier1Power { get; internal set; } = 4;
		[JsonProperty] public int Tier2Power { get; internal set; } = 3 * 3 - 1;
		[JsonProperty] public int Tier3Power { get; internal set; } = 5 * 5 - 1;
		[JsonProperty] public int Tier4Power { get; internal set; } = 7 * 7 - 1;
		[JsonProperty] public int Tier5Power { get; internal set; } = 9 * 9 - 1;
		[JsonProperty] public int Tier6Power { get; internal set; } = 11 * 11 - 1;
		[JsonProperty] public int Tier7Power { get; internal set; } = 13 * 13 - 1;
		[JsonProperty] public int Tier8Power { get; internal set; } = 15 * 15 - 1;
		[JsonProperty] public bool CompatibilityMode { get; internal set; } = true;
		[JsonProperty] public bool WaterGardenPots { get; internal set; } = false;
		[JsonProperty] public bool WaterPetBowl { get; internal set; } = false;
	}
}