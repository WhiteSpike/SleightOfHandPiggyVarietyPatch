using BepInEx;
using BepInEx.Logging;
using SleightOfHandPiggyVarietyPatch.Misc;
using HarmonyLib;
using SleightOfHandPiggyVarietyPatch.Patches;
namespace SleightOfHandPiggyVarietyPatch
{
    [BepInPlugin(Metadata.GUID,Metadata.NAME,Metadata.VERSION)]
    [BepInDependency(PiggyVarietyMod.Plugin.modGUID)]
    [BepInDependency("com.malco.lethalcompany.moreshipupgrades")]
    public class Plugin : BaseUnityPlugin
    {
        internal const string ITEM_NAME = "Peeper";
        internal static readonly Harmony harmony = new(Metadata.GUID);
        internal static readonly ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource(Metadata.NAME);

        void Awake()
        {
            harmony.PatchAll(typeof(M4ItemPatcher));
            mls.LogInfo("Patched the rifle succesfully");
            harmony.PatchAll(typeof(RevolverItemPatcher));
            mls.LogInfo("Patched the revolver successfully");

            mls.LogInfo($"{Metadata.NAME} {Metadata.VERSION} has been loaded successfully.");
        }
    }   
}
