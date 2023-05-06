using System.Collections.Generic;

namespace LocalizationExtension.ModSource {
	public interface IModSource {
		IEnumerable<LocalMod> GetMods();
	}
}