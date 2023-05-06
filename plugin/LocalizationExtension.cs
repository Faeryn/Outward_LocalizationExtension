using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace LocalizationExtension {
	[BepInPlugin(GUID, NAME, VERSION)]
	public class LocalizationExtension : BaseUnityPlugin {
		public const string GUID = "faeryn.localizationextension";
		public const string NAME = "LocalizationExtension";
		public const string VERSION = "1.0.0";
		internal static ManualLogSource Log;
		internal static LocalizationExtension Instance;

		internal readonly ModLocalizationManager ModLocalizationManager = new ModLocalizationManager();

		internal void Awake() {
			Instance = this;
			Log = this.Logger;
			Log.LogMessage($"Starting {NAME} {VERSION}");
			new Harmony(GUID).PatchAll();
		}
	}
}