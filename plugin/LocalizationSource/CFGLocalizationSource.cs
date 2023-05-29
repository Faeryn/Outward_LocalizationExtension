using System.Collections.Generic;
using System.IO;
using LocalizationExtension.Model;

namespace LocalizationExtension.LocalizationSource; 

public class CFGLocalizationSource : FileLocalizationSource {
	public CFGLocalizationSource(string path) : base(path) {
	}
	
	public override bool TryGetLocalization(out Localization localization) {
		if (!File.Exists(Path)) {
			localization = null;
			return false;
		}
		LocalizationExtension.Log.LogInfo($"Loading localization file: {Path}");
		
		localization = new Localization();
		Dictionary<int, Model.Item> items = new();
		foreach (string line in File.ReadLines(Path)) {
			if (line.StartsWith(";") || line.StartsWith("#") || !line.Contains("=")) {
				continue;
			}
			string[] split = line.Split(new []{'='}, 2);
			string key = split[0].ToLower();
			string value = split[1];
			
			// Dialogue
			if (key.StartsWith("/dialogue/")) {
				// TODO Dialogue localization
				continue;
			}
		
			// Item
			if (key.StartsWith("/item/")) {
				ProcessItem(key, value, localization, items);
				continue;
			}

			// General
			localization.General.Add(new General(key, value));
		}
		
		localization.Item.AddRange(items.Values);
		
		return true;
	}

	private void ProcessItem(string key, string value, Localization localization, Dictionary<int, Model.Item> items) {
		string[] keySplit = key.Split('/');
		int itemId;
		if (keySplit.Length != 4 // 0 is empty
			|| !int.TryParse(keySplit[2], out itemId) 
			|| (keySplit[3] != "name" && keySplit[3] != "description")) {
			LocalizationExtension.Log.LogError($"Invalid item localization key: {key}");
			return;
		}

		string type = keySplit[3];

		string name = type == "name" ? value : null;
		string description = type == "description" ? value : null;

		Model.Item item;
		if (items.TryGetValue(itemId, out item)) {
			if (name != null) {
				item.Name = name;
			}
			if (description != null) {
				item.Description = description;
			}
			items[itemId] = item;
		} else {
			items[itemId] = new Model.Item(itemId, name, description);
		}
	}
	
}