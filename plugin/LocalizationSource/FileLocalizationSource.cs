using LocalizationExtension.Model;

namespace LocalizationExtension.LocalizationSource; 

public abstract class FileLocalizationSource : LocalizationSource {
	protected string Path;

	protected FileLocalizationSource(string path) {
		Path = path;
	}

	public abstract bool TryGetLocalization(out Localization localization);
}