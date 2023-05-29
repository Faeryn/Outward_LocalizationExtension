using HarmonyLib;

namespace LocalizationExtension.Patches; 

[HarmonyPatch(typeof(LocalizationManager))]
public static class LocalizationManagerPatches {
	[HarmonyPatch(nameof(LocalizationManager.StartLoading)), HarmonyFinalizer]
	private static void LocalizationManager_StartLoading_Finalizer(LocalizationManager __instance) {
		LocalizationExtension.Instance.ModLocalizationManager.InjectLocalization(__instance, __instance.CurrentLanguageDefaultName);
	}
}