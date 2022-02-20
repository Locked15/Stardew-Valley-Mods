﻿using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shockah.ProjectFluent
{
	public class ProjectFluent: Mod
	{
		public static ProjectFluent Instance { get; private set; }
		public IFluentApi Api { get; private set; }

		internal ModConfig Config { get; private set; }
		internal IFluent<string> Fluent { get; private set; }
		internal IEnumFluent<ContentPatcherPatchingMode> FluentContentPatcherPatchingMode { get; private set; }

		private Func<IManifest, string> getModDirectoryPath;

		private readonly IFluent<string> noOpFluent = new NoOpFluent();
		private readonly IDictionary<(string modID, string name), IDictionary<IGameLocale, IFluent<string>>> localizationCache = new Dictionary<(string modID, string name), IDictionary<IGameLocale, IFluent<string>>>();

		public override void Entry(IModHelper helper)
		{
			Instance = this;
			Api = new FluentApi(this);
			Config = helper.ReadConfig<ModConfig>();
			Fluent = Api.GetLocalizationsForCurrentLocale<string>(ModManifest);
			FluentContentPatcherPatchingMode = Api.GetEnumFluent<ContentPatcherPatchingMode>(Fluent, "config-contentPatcherPatchingMode-value.");
			helper.Events.GameLoop.GameLaunched += OnGameLaunched;

			var directoryPathGetter = AccessTools.PropertyGetter(Type.GetType("StardewModdingAPI.Framework.IModMetadata, StardewModdingAPI"), "DirectoryPath");
			getModDirectoryPath = (manifest) =>
			{
				var modInfo = helper.ModRegistry.Get(manifest.UniqueID);
				return (string)directoryPathGetter.Invoke(modInfo, null);
			};
		}

		public override object GetApi() => Api;

		private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
		{
			var harmony = new Harmony(ModManifest.UniqueID);
			ContentPatcherIntegration.Setup(harmony);

			SetupConfig();
		}

		private void SetupConfig()
		{
			var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

			static IEnumerable<string> GetBuiltInLocales()
			{
				foreach (var value in Enum.GetValues(typeof(LocalizedContentManager.LanguageCode)))
				{
					var typedValue = (LocalizedContentManager.LanguageCode)value;
					if (typedValue == LocalizedContentManager.LanguageCode.mod)
						continue;
					yield return typedValue == LocalizedContentManager.LanguageCode.en ? "en-US" : Game1.content.LanguageCodeString(typedValue);
				}
			}

			configMenu?.Register(
				ModManifest,
				reset: () => Config = new ModConfig(),
				save: () => Helper.WriteConfig(Config)
			);

			configMenu?.AddTextOption(
				mod: ModManifest,
				name: () => Fluent["config-contentPatcherPatchingMode"],
				tooltip: () => Fluent["config-contentPatcherPatchingMode.tooltip"],
				getValue: () => FluentContentPatcherPatchingMode[Config.ContentPatcherPatchingMode],
				setValue: value => Config.ContentPatcherPatchingMode = FluentContentPatcherPatchingMode.GetFromLocalizedName(value),
				allowedValues: FluentContentPatcherPatchingMode.GetAllLocalizedNames().ToArray()
			);

			configMenu?.AddTextOption(
				mod: ModManifest,
				name: () => Fluent["config-localeOverride"],
				tooltip: () => Fluent["config-localeOverride.tooltip"],
				getValue: () => String.IsNullOrEmpty(Config.CurrentLocaleOverride) ? "" : Config.CurrentLocaleOverride,
				setValue: value => Config.CurrentLocaleOverride = String.IsNullOrEmpty(value) ? null : value,
				fieldId: "localeOverride-custom"
			);

			configMenu?.AddParagraph(
				mod: ModManifest,
				text: () => Fluent.Get("config-localeOverride.subtitle", new { Values = GetBuiltInLocales().Join() })
			);
		}

		private IEnumerable<string> GetFilePathCandidates(IManifest mod, string name, IGameLocale locale)
		{
			var baseModPath = getModDirectoryPath(mod);
			if (baseModPath == null)
				yield break;

			foreach (var relevantLocale in locale.GetRelevantLanguageCodes())
			{
				var fileNameWithoutExtension = $"{(String.IsNullOrEmpty(name) ? "" : $"{name}.")}{relevantLocale}";
				yield return Path.Combine(baseModPath, "i18n", $"{fileNameWithoutExtension}.ftl");
			}
		}

		#region APIs

		public IGameLocale CurrentLocale
		{
			get
			{
				return LocalizedContentManager.CurrentLanguageCode switch
				{
					LocalizedContentManager.LanguageCode.mod => new IGameLocale.Mod(LocalizedContentManager.CurrentModLanguage),
					_ => new IGameLocale.BuiltIn(LocalizedContentManager.CurrentLanguageCode),
				};
			}
		}

		public IFluent<Key> GetLocalizations<Key>(IGameLocale locale, IManifest mod, string name = null)
		{
			var rootKey = (modID: mod.UniqueID, name: name);
			if (!localizationCache.ContainsKey(rootKey))
				localizationCache[rootKey] = new Dictionary<IGameLocale, IFluent<string>>();

			var specificCache = localizationCache[rootKey];
			if (!specificCache.ContainsKey(locale))
				specificCache[locale] = new FileResolvingFluent(locale, GetFilePathCandidates(mod, name, locale), noOpFluent);

			var baseFluent = specificCache[locale];
			return typeof(Key) == typeof(string) ? (IFluent<Key>)baseFluent : new MappingFluent<Key>(baseFluent);
		}

		#endregion
	}
}