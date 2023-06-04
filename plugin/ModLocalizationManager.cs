using System.Collections.Generic;
using System.IO;
using LocalizationExtension.LocalizationSource;
using LocalizationExtension.Model;
using LocalizationExtension.ModSource;
using Localizer;

namespace LocalizationExtension {
	
	internal class ModLocalizationManager {
		private const string DEFAULT_LANGUAGE = "English";

		private ModListManager modListManager = new ModListManager();

		internal void InjectLocalization(LocalizationManager localizationManager) {
			string language = localizationManager.CurrentLanguageDefaultName;
			foreach (ModInfo modInfo in modListManager.ModList) {
				if (!modInfo.IsLocal || !Directory.Exists(Path.Combine(modInfo.LocalPath, "lang"))) {
					continue;
				}

				string modName = modInfo.GUID ?? modInfo.Name;
				
				Localization localization;
				bool loaded;

				if (!(loaded = TryLoadLanguageFile(modInfo.LocalPath, language, out localization))) {
					string fallbackLanguage = GetFallbackLanguage(modInfo.LocalPath);
					LocalizationExtension.Log.LogInfo($"{modName}: Language {language} not supported, using fallback language: {fallbackLanguage}");
					loaded = TryLoadLanguageFile(modInfo.LocalPath, fallbackLanguage, out localization);
				}

				if (!loaded) {
					continue;
				}

				// General
				LocalizationExtension.Log.LogInfo($"{modName}: Loading {localization.General.Count} general localization entries");
				localization.General.ForEach(generalLoc => AddGeneralLocalization(localizationManager.m_generalLocalization, modInfo.GUID, generalLoc));
				
				// Item
				LocalizationExtension.Log.LogInfo($"{modName}: Loading {localization.Item.Count} item localization entries");
				localization.Item.ForEach(itemLoc => AddItemLocalization(localizationManager.m_itemLocalization, itemLoc));
				
				// Dialogue
				// TODO Dialogue localization
			}
		}

		string GetFallbackLanguage(string modPath) {
			string filePath = Path.Combine(modPath, "lang", "default.txt");
			if (!File.Exists(filePath)) {
				return DEFAULT_LANGUAGE;
			}
			return File.ReadAllText(filePath).Trim();
		}

		void AddItemLocalization(IDictionary<int, ItemLocalization> itemLocalization, Model.Item item) {
			itemLocalization[item.ItemID] = new ItemLocalization(item.Name, item.Description);
			LocalizationExtension.Log.LogDebug($"{item.ItemID}: name='{item.Name}', description='{item.Description}'");
		}

		void AddGeneralLocalization(IDictionary<string, string> generalLocalization, string modPrefix, General general) {
			string key = general.Key;
			if (key.StartsWith("/") || modPrefix == null) {
				key = key.Substring(1);
			} else {
				key = modPrefix + "." + key;
			}
			generalLocalization[key] = general.Value;
			LocalizationExtension.Log.LogDebug($"'{key}'='{general.Value}'");
		}

		bool TryLoadLanguageFile(string modPath, string language, out Localization localization) {
			string basePath = Path.Combine(modPath, "lang", language);
			if (new CFGLocalizationSource(basePath + ".cfg").TryGetLocalization(out localization)) {
				return true;
			}
			if (new CFGLocalizationSource(basePath + "_g.cfg").TryGetLocalization(out localization)) {
				LocalizationExtension.Log.LogWarning("The _g suffix for language files is deprecated and should be removed.");
				return true;
			}
			return false;
		}
	}
}