using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using SideLoader;

namespace LocalizationExtension {
	[BepInDependency(SIDELOADER_GUID, BepInDependency.DependencyFlags.SoftDependency)]
	[BepInPlugin(GUID, NAME, VERSION)]
	public class LocalizationExtension : BaseUnityPlugin {
		public const string GUID = "faeryn.localizationextension";
		public const string NAME = "LocalizationExtension";
		public const string VERSION = "1.1.0";
		public const string SIDELOADER_GUID = "com.sinai.SideLoader";
		internal static ManualLogSource Log;
		internal static LocalizationExtension Instance;

		internal readonly ModLocalizationManager ModLocalizationManager = new ModLocalizationManager();

		internal void Awake() {
			Instance = this;
			Log = this.Logger;
			Log.LogMessage($"Starting {NAME} {VERSION}");
			new Harmony(GUID).PatchAll();
			if (Chainloader.PluginInfos.ContainsKey(SIDELOADER_GUID)) {
				InitializeSideLoaderCompatibility();
			}
		}

		private void InitializeSideLoaderCompatibility() {
			Log.LogInfo("Initializing SideLoader compatibility fix");
			SL.OnPacksLoaded += SL_OnOnPacksLoaded;
		}

		private void SL_OnOnPacksLoaded() {
			// Quick and dirty SideLoader compatibility fix: inject localization (again) after SL packs are loaded
			ModLocalizationManager.InjectLocalization(LocalizationManager.Instance);
		}
	}
}