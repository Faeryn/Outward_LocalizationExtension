using LocalizationExtension.Model;

namespace LocalizationExtension.LocalizationSource; 

public interface LocalizationSource {
	bool TryGetLocalization(out Localization localization);
}