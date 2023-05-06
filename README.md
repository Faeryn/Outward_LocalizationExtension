# LocalizationExtension

Allows modders to provide localization for their mods using the game's localization system.

## Usage
### For mod developers
Add this mod as dependency to your mod's Thunderstore manifest.  
Example:
```
"dependencies": [
    "BepInEx-BepInExPack_Outward-5.4.18",
    "Faeryn-LocalizationExtension-1.0.0"
]
```
Add one or more language files to your mod's `lang` folder (you will have to create it).  
The files should be in the following format: `languagekey_type.ext`, where
* **languagekey** is one of the game's supported languages: `English, French, German, Italian, Spanish, zeChinese, zeJapanese, zeKorean, zePolish, zePortuguese, zeRussian, zeTurkish`
* **type** is a letter for the localization type: `g, i, d` - General, Item, Dialogue. Note: Currently only `g` aka. General is supported.  
* **ext** is the extension, currently only `cfg` is supported.

Example:
```
MyMod.zip
|- plugins
|  |- MyMod.dll
|  -- lang
|     |- English_g.cfg
|     |- Spanish_g.cfg
|     -- French_g.cfg
|- icon.png
|- manifest.json
-- README.md
```
The contents of a cfg file is a list of key-value pairs separated by `=`. Keys are WITHOUT your mod's GUID. For example:
```
some.key=Some Value
some.key.with_parameter=Some Key With {1} Parameter {2}
```
Note: Parameter numbering starts from 1, not 0.  

Then you can use the game's localization manager for texts in your mod, like so:
```
LocalizationManager.Instance.GetLoc($"{YourMod.GUID}.some.key");
```
Or with parameters:
```
LocalizationManager.Instance.GetLoc($"{YourMod.GUID}.some.key.with_parameter", "param1", "param2");
```
In case you don't provide a language file for each language, you should also add a file named `default.txt` into the `lang` folder
containing the name of your fallback language (which will be used for unsupported languages). Default is `English` if the file is missing.

### For everyone else
If you are not a mod developer, you can most likely ignore this mod.  
The only exception is if you are manually installing mods (instead of using the Thunderstore mod manager)
in which case download this mod and put it into your BepInEx plugins folder.

## Planned features
- Support for Dialog and Item localization
- Support for json and xml language file formats
- Support for SideLoader mods

## Changelog

### 1.0.0
- Initial release