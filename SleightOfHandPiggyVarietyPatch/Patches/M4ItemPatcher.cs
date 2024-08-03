using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Shotgun;
using PiggyVarietyMod.Patches;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SleightOfHandPiggyVarietyPatch.Patches
{
    [HarmonyPatch(typeof(M4Item))]
    internal static class M4ItemPatcher
    {
        [HarmonyPatch(nameof(M4Item.ReloadGunAnimation))]
        [HarmonyPrefix]
        static void ReloadGunAnimationPrefix(M4Item __instance)
        {
            __instance.playerHeldBy.playerBodyAnimator.speed *= 2f + SleightOfHand.ComputeSleightOfHandSpeedBoost();
            __instance.gunAnimator.speed *= 2f + SleightOfHand.ComputeSleightOfHandSpeedBoost();
            __instance.StartCoroutine(waitToEndReloadAnimation(__instance));
        }
        [HarmonyPatch(nameof(M4Item.ReloadGunAnimation), MethodType.Enumerator)]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> ReloadGunAnimationTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo getSleightOfHandSpeedBoost = typeof(SleightOfHand).GetMethod(nameof(SleightOfHand.GetSleightOfHandSpeedBoost));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindFloat(ref index, ref codes, findValue: 2f, addCode: getSleightOfHandSpeedBoost, errorMessage: "Couldn't find the first WaitForSecond value");
            Tools.FindFloat(ref index, ref codes, findValue: 0.55f, addCode: getSleightOfHandSpeedBoost, errorMessage: "Couldn't find the second WaitForSecond value");

            return codes;
        }

        static IEnumerator waitToEndReloadAnimation(this M4Item gun)
        {
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => gun.isReloading);
            gun.playerHeldBy.playerBodyAnimator.speed /= 2f + SleightOfHand.ComputeSleightOfHandSpeedBoost();
            gun.gunAnimator.speed /= 2f + SleightOfHand.ComputeSleightOfHandSpeedBoost();
        }
    }
}
