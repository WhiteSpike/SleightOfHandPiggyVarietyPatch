using HarmonyLib;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Shotgun;
using PiggysVarietyMod.Items;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SleightOfHandPiggyVarietyPatch.Patches
{
    [HarmonyPatch(typeof(PiggysVarietyMod.Items.RifleScript))]
    internal static class M4ItemPatcher
    {
        [HarmonyPatch(nameof(RifleScript.FinishReloadCoroutine))]
        [HarmonyPrefix]
        static void FinishReloadCoroutinePrefix(RifleScript __instance)
        {
            if (!BaseUpgrade.GetActiveUpgrade(SleightOfHand.UPGRADE_NAME)) return;
            __instance.playerHeldBy.playerBodyAnimator.speed *= 2f + SleightOfHand.ComputeSleightOfHandSpeedBoost();
            __instance.gunAnimator.speed *= 2f + SleightOfHand.ComputeSleightOfHandSpeedBoost();
            __instance.StartCoroutine(waitToEndReloadAnimation(__instance));
        }
        [HarmonyPatch(nameof(RifleScript.StartReload))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> StartReloadTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo getSleightOfHandSpeedBoost = typeof(SleightOfHand).GetMethod(nameof(SleightOfHand.GetSleightOfHandSpeedBoost));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindLocalField(ref index, ref codes, localIndex: 0, addCode: getSleightOfHandSpeedBoost);

            return codes;
        }

        static IEnumerator waitToEndReloadAnimation(this RifleScript gun)
        {
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => gun.isReloading);
            gun.playerHeldBy.playerBodyAnimator.speed /= 2f + SleightOfHand.ComputeSleightOfHandSpeedBoost();
            gun.gunAnimator.speed /= 2f + SleightOfHand.ComputeSleightOfHandSpeedBoost();
        }
    }
}
