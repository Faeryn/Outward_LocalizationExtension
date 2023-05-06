using System;
using System.Linq;
using HarmonyLib;

namespace LocalizationExtension.Patches {
	[HarmonyPatch(typeof(LocalizationManager))]
	public static class LocalizationManagerPatches {
		[HarmonyPatch(nameof(LocalizationManager.LoadGeneralLocalization)), HarmonyPrefix]
		private static void LocalizationManager_LoadGeneralLocalization_Postfix(LocalizationManager __instance) {
			LocalizationExtension.Instance.ModLocalizationManager.InjectGeneralLocalization(__instance.m_generalLocalization, __instance.CurrentLanguageDefaultName);
		}
	}
}