using System.Collections.Generic;
using System.IO;
using LocalizationExtension.ModSource;

namespace LocalizationExtension {
	
	internal class ModLocalizationManager {
		private const string DEFAULT_LANGUAGE = "English";
		
		private ModListManager modListManager = new ModListManager();

		internal void InjectGeneralLocalization(IDictionary<string, string> generalLocalization, string language) {
			foreach (ModInfo modInfo in modListManager.ModList) {
				if (!modInfo.IsLocal || modInfo.GUID == null || !Directory.Exists(Path.Combine(modInfo.LocalPath, "lang"))) {
					continue;
				}
				
				if (!TryLoadLanguageFiles(modInfo.LocalPath, language, modInfo.GUID, generalLocalization)) {
					string fallbackLanguage = GetFallbackLanguage(modInfo.LocalPath);
					LocalizationExtension.Log.LogInfo($"{modInfo.GUID}: Language {language} not supported, using fallback language: {fallbackLanguage}");
					TryLoadLanguageFiles(modInfo.LocalPath, fallbackLanguage, modInfo.GUID, generalLocalization);
				}
			}
		}

		string GetFallbackLanguage(string modPath) {
			string filePath = Path.Combine(modPath, "lang", "default.txt");
			if (!File.Exists(filePath)) {
				return DEFAULT_LANGUAGE;
			}
			return File.ReadAllText(filePath).Trim();
		}

		bool TryLoadLanguageFiles(string modPath, string language, string modPrefix, IDictionary<string, string> generalLocalization) {
			string basePath = Path.Combine(modPath, "lang", language + "_g");
			if (TryLoadCfg(basePath + ".cfg", modPrefix, generalLocalization)) {
				return true;
			}
			if (TryLoadJson(basePath + ".json", modPrefix, generalLocalization)) {
				return true;
			}
			return false;
		}

		bool TryLoadCfg(string file, string modPrefix, IDictionary<string, string> generalLocalization) {
			if (!File.Exists(file)) {
				return false;
			}
			LocalizationExtension.Log.LogInfo($"{modPrefix}: Loading general localization cfg: {file}");
			foreach (string line in File.ReadLines(file)) {
				string[] split = line.Split(new []{'='}, 2);
				generalLocalization.Add(modPrefix + "." + split[0], split[1]);
			}
			return true;
		}

		bool TryLoadJson(string file, string modPrefix, IDictionary<string, string> generalLocalization) {
			if (!File.Exists(file)) {
				return false;
			}
			LocalizationExtension.Log.LogInfo($"{modPrefix}: Loading general localization json: {file}");
			// TODO json deserialization
			return false;
		}
	}
}