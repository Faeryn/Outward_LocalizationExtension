# LocalizationExtension

Allows modders to provide localization for their mods using the game's localization system.

**If you are not a mod developer**, you can most likely ignore this mod.  
The only exception is if you are manually installing mods (instead of using the Thunderstore mod manager)
in which case download this mod and put it into your BepInEx plugins folder.


### SideLoader compatibility
SideLoader already has its own localization table patcher, and it does not support different languages. My current workaround to
that is not perfect, you may have to **restart the game after changing language**.

## Usage
### General usage
Add this mod as dependency to your mod's Thunderstore manifest.  
Example:
```
"dependencies": [
    "BepInEx-BepInExPack_Outward-5.4.18",
    "Faeryn-LocalizationExtension-1.1.3"
]
```
Add one or more language files to your mod's `lang` folder (you will have to create it).  
The files should be in the following format: `languagekey.ext`, where
* **languagekey** is one of the game's supported languages: `English, French, German, Italian, Spanish, zeChinese, zeJapanese, zeKorean, zePolish, zePortuguese, zeRussian, zeTurkish`
* **ext** is the extension, currently only `cfg` is supported.

Example:
```
MyMod.zip
|- plugins
|  |- MyMod.dll
|  -- lang
|     |- English.cfg
|     |- Spanish.cfg
|     -- French.cfg
|- icon.png
|- manifest.json
-- README.md
```
The contents of a cfg file is a list of key-value pairs separated by `=`. Keys are WITHOUT your mod's GUID. For example:
```
some.key=Some Value
some.key.with_parameter=Some Key With {1} Parameter {2}
```
*Note: Parameter numbering starts from 1, not 0.  *

Empty lines and lines starting with `#` or `;` are ignored so you can also add comments.  
The cfg format doesn't allow newlines (each line is a single entry), therefore if you want to add newlines, you will have to use \n, like so:
```
some.key.with_newline=This is the first line\nThis is the second line\n\nThird line is empty, this is the fourth
```

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

### Items and skills
For item and skill localization, the key should be in the following format:
```
/item/[itemID]/name
/item/[itemID]/description
```
Where `[itemID]` is the ID of the item or skill in question. Note that this will override the name and description specified in the SideLoader item definition.  
Example:
```
/item/-1234567/name=Example Item
/item/-1234567/description=An item that serves as a good example
```

### Enchantments
Enchantments added by SideLoader require the following key format:
```
/Enchantment_[enchantmentID]
/DESC_Enchantment_[enchantmentID]
```
Where `[enchantmentID]` is the ID of the enchantment in question. Note that this will override the name and description specified in the SideLoader enchantment definition.

Example:
```
/Enchantment_-1234567=Example Enchantment
/DESC_Enchantment_-1234567=This is an example enchantment
```

### Status effects
Status effects added by SideLoader require the following key format:
```
/NAME_[StatusIdentifier]
/DESC_[StatusIdentifier]
```
Where `[StatusIdentifier]` is the `StatusIdentifier` of the status effect in question (defined in the `TargetStatusIdentifier` field). Note that this will override the name and description specified in the SideLoader status effect definition.

Example:
```
/NAME_ExampleStatusEffect=Example Status Effect
/DESC_ExampleStatusEffect=A good example
```

### Overriding built-in localization entries
By default, **general** localization entries are prefixed by mod GUID. If you, for some reason, need to override existing entries, 
then the `/` prefix can be used. For example:
```
/examplekey
```
This will override the general localization entry `examplekey`.  
*Note: Currently this works reliably only for vanilla Outward entries, unless noted. Overriding other mods' entries may or may not work.*

### Localization of other mods
It is possible to localize other SideLoader mods that do not use LocalizationExtension. Only **items/skills**, **effects** and **enchantments** can be localized this way,
using the aforementioned override mechanism.  
If the target mod uses LocalizationExtension, then the additional languages will be added seamlessly. Trying to override existing 
languages (for example the original mod has German language support, and you are also adding a German language file) may or may not work.

Simply create a mod, as described in the **General usage** section while excluding the mod dll (or any SideLoader resource):
```
ModName_Localization.zip
|- plugins
|  -- lang
|     |- English.cfg
|     |- Spanish.cfg
|     -- French.cfg
|- icon.png
|- manifest.json
-- README.md
```
In the manifest file include the original mod too, as dependency:
```
"dependencies": [
    "BepInEx-BepInExPack_Outward-5.4.18",
    "Faeryn-LocalizationExtension-1.1.3",
    "OtheMod-OtherAuthor-1.0.0"
]
```
This way your mod will be compatible with the original mod even as new versions are released, since you are only patching the localization tables.

*Note: It's a good idea to distinguish your localization mod from the original, for example by adding your language suffix to the name "OtherMod_SP",
and making sure the description is clear about the fact that this is only a localization patch, NOT the full mod.*

Example mod: https://github.com/Faeryn/Outward_LocalizationExampleMod

## Planned features
- Support for Dialog localization
- Support for json and xml language file formats
- Ability to localize mods by direct text matching

## Changelog

### 1.1.3
- Custom items and skills (using SideLoader) should now have their localized values everywhere (incl. trainer skill trees)

### 1.1.2
- Allow newlines in single-line localized text
- Non-BepInEx mods are also scanned for language files. Note that they need a Thunderstore manifest present for this feature to work.

### 1.1.1
- Fix for vanilla overrides

### 1.1.0
- Item localization
- Enchantment localization
- Support for SideLoader mods
- Allow comments in cfg (`#` or `;`)
- Ability to override vanilla Outward entries (`/` prefix)

### 1.0.0
- Initial release