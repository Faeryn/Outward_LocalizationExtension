using ExitGames.Client.Photon;
using Version = System.Version;

namespace LocalizationExtension.ModSource {
	public class ModInfo {
		public string GUID { get; }
		public string Name { get; }
		public Version Version { get; }
		public string Author { get; }
		public string Description { get; }
		public string WebsiteURL { get; }
		public string LocalPath { get; }
		
		public bool IsLocal => LocalPath != null;

		public ModInfo(string guid, string name, Version version, string author, string description, string websiteURL, string localPath) {
			GUID = guid;
			Name = name;
			Version = version;
			Author = author;
			Description = description;
			WebsiteURL = websiteURL;
			LocalPath = localPath;
		}
		
		public ModInfo Merge(ModInfo modInfo) {
			return new ModInfo(
				GUID ?? modInfo.GUID,
				Name ?? modInfo.Name,
				Version ?? modInfo.Version,
				Author ?? modInfo.Author,
				Description ?? modInfo.Description,
				WebsiteURL ?? modInfo.WebsiteURL,
				LocalPath ?? modInfo.LocalPath
			);
		}

		/**
		 * Used for Photon serialization
		 */
		public Hashtable ToDict() {
			Hashtable dict = new Hashtable();
			dict.Add("GUID", GUID);
			dict.Add("Name", Name);
			dict.Add("Version", Version.ToString());
			dict.Add("Author", Author);
			dict.Add("Description", Description);
			dict.Add("WebsiteURL", WebsiteURL);
			// No local path for remote mods
			return dict;
		}

		/**
		 * Used for Photon deserialization
		 */
		public static ModInfo FromDict(Hashtable dict) {
			return new ModInfo((string)dict["GUID"], 
				(string)dict["Name"], 
				Version.Parse((string)dict["Version"]),
				(string)dict["Author"],
				(string)dict["Description"],
				(string)dict["WebsiteURL"],
				null // No local path for remote mods
				);
		}

		public override string ToString() {
			return $"ModInfo{{{nameof(GUID)}: {GUID}, {nameof(Name)}: {Name}, {nameof(Version)}: {Version}, {nameof(Author)}: {Author}}}";
		}
	}
}