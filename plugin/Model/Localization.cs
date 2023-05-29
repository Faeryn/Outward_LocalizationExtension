using System.Collections.Generic;

namespace LocalizationExtension.Model; 

public class Localization {
	public readonly List<General> General = new();
	public readonly List<Item> Item = new();
}