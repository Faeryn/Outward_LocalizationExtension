using System.Collections.Generic;

//TODO Move this entire thing into its own library, shared with ModSupport
namespace LocalizationExtension.ModSource {
	public class ModListManager {
		private List<IModSource> modSources = new List<IModSource> {
			new ThunderstoreManifestModSource(),
			new BepInExModSource()
		};

		private ModList modList;

		public ModList ModList {
			get {
				if (modList == null) {
					Initialize();
				}
				return modList;
			}
		}

		private void Initialize() {
			Dictionary<string, ModInfo> mods = new Dictionary<string, ModInfo>();
			foreach (IModSource modSource in modSources) {
				LoadMods(modSource, mods);
			}
			LocalizationExtension.Log.LogDebug($"##### Found {mods.Count} mods #####");
			foreach (KeyValuePair<string, ModInfo> mod in mods) {
				LocalizationExtension.Log.LogDebug($"'{mod.Key}' - {mod.Value}");
			}
			modList = new ModList(mods.Values);
		}

		private void LoadMods(IModSource modSource, Dictionary<string, ModInfo> mods) {
			foreach (LocalMod newMod in modSource.GetMods()) {
				string modKey = newMod.FolderName;
				if (modKey == null) {
					modKey = newMod.DllName;
				}
				if (modKey == null) {
					// Fallback option in case folderName or dllName is not available for some reason
					// Not ideal but at this point it should be fine
					modKey = newMod.ModInfo.Name;
				}
				if (modKey == null) {
					LocalizationExtension.Log.LogError($"Unable to determine mod root path or name for mod: {newMod.ModInfo}");
					continue;
				}
				if (!mods.ContainsKey(modKey)) {
					mods.Add(modKey, newMod.ModInfo);
				} else {
					mods[modKey] = mods[modKey].Merge(newMod.ModInfo);
				}
			}
		}
	}
}