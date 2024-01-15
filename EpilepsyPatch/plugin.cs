using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using EpilepsyPatch.patches;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EpilepsyPatch
{

    [BepInPlugin(modGUID, modName, modVersion)]

    public class EpilepsyPatchBase : BaseUnityPlugin
    {
        private const string modGUID = "LongParsnip.EpilepsyPatch";
        private const string modName = "EpilepsyPatch";
        private const string modVersion = "1.0.5.0";
        public const bool LogDebugMessages = false;                     //This is for helping with developing the transpiler code, to find the correct IL to modify.

        private readonly Harmony harmony = new Harmony(modGUID);
        private static EpilepsyPatchBase Instance;

        private ManualLogSource mls;

        //Config setting keys.
        public static string StunGrenadeExplosionDisabledKey = "Disable stun grenade explosion";
        public static string StunGrenadeFilterDisabledKey = "Disable stun grenade flashed effect";
        public static string ScanBlueFlashDisabledKey = "Disable scan blue flash effect";
        public static string GettingFiredLightDisabledKey = "Getting fired red flashing light disabled";
        public static string DisableGlobalNotificationsKey = "Disable global notifications";
        public static string DisableRadiationWarningKey = "Disable radiation warning";
        public static string ReplaceWarningWithHintKey = "Replace warning with hint";
        public static string ForceShipLightsOnKey = "Force ship lights on";
        public static string HideLightningStrikesKey = "Hide lightning strikes";
        public static string HideLightningExplosionsKey = "Hide lightning explosions";

        //Config Entries.
        public static ConfigEntry<bool> StunGrenadeExplosionDisabled;
        public static ConfigEntry<bool> StunGrenadeFilterDisabled;
        public static ConfigEntry<bool> ScanBlueFlashDisabled;
        public static ConfigEntry<bool> GettingFiredLightDisabled;
        public static ConfigEntry<bool> DisableGlobalNotifications;
        public static ConfigEntry<bool> DisableRadiationWarning;
        public static ConfigEntry<bool> ReplaceWarningWithHint;
        public static ConfigEntry<bool> ForceShipLightsOn;
        public static ConfigEntry<bool> HideLightningStrikes;
        public static ConfigEntry<bool> HideLightningExplosions;

        void Awake()
        {

            //Config bindings.
            StunGrenadeExplosionDisabled = (Config.Bind<bool>("General", StunGrenadeExplosionDisabledKey, true, new ConfigDescription("Should the stun grenade explosion animation be disabled.")));
            StunGrenadeFilterDisabled = (Config.Bind<bool>("General", StunGrenadeFilterDisabledKey, false, new ConfigDescription("Should the stun grenade stunned filter be disabled.")));
            ScanBlueFlashDisabled = (Config.Bind<bool>("General", ScanBlueFlashDisabledKey, true, new ConfigDescription("Should the blue flash when scanning be disabled.")));
            GettingFiredLightDisabled = (Config.Bind<bool>("General", GettingFiredLightDisabledKey, true, new ConfigDescription("Should the red light when getting fired be disabled")));
            DisableGlobalNotifications = (Config.Bind<bool>("General", DisableGlobalNotificationsKey, false, new ConfigDescription("Should global notifications be disabled")));
            DisableRadiationWarning = (Config.Bind<bool>("General", DisableRadiationWarningKey, true, new ConfigDescription("Should the radiation warning be disabled")));
            ReplaceWarningWithHint = (Config.Bind<bool>("General", ReplaceWarningWithHintKey, true, new ConfigDescription("Should warnings be replaced with hints")));
            ForceShipLightsOn = (Config.Bind<bool>("General", ForceShipLightsOnKey, true, new ConfigDescription("Should ship lights always be forced to be on")));
            HideLightningStrikes = (Config.Bind<bool>("General", HideLightningStrikesKey, true, new ConfigDescription("Should lightning strikes be hidden")));
            HideLightningExplosions = (Config.Bind<bool>("General", HideLightningExplosionsKey, true, new ConfigDescription("Should explosions from lightning strikes be hidden")));



            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("EpilepsyPatch is awake and needs coffee.");

            harmony.PatchAll(typeof(StunGrenadePatch));
            harmony.PatchAll(typeof(StunGrenadeExplosionPatch));
            harmony.PatchAll(typeof(PingScan_performedPatch));
            harmony.PatchAll(typeof(DisplayGlobalNotification_Patch));
            harmony.PatchAll(typeof(RadiationWarningHUD_Patch));
            harmony.PatchAll(typeof(DisplayTip_Patch));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
            //harmony.PatchAll(typeof(ShipTeleporterPatch));        //Example of how to use a transpiler on a coroutine.
            harmony.PatchAll(typeof(LightningPatch));
            //harmony.PatchAll(typeof(ExplosionPatch));     //didn't work for lightning, might be useful for landmines though, needs more testing.
        }


        public static bool IsRandomRangeCall(CodeInstruction instruction)
        {
            // Check if the instruction is a call to UnityEngine.Random.Range
            return instruction.opcode == OpCodes.Call &&
                   instruction.operand is MethodInfo methodInfo &&
                   methodInfo.DeclaringType == typeof(UnityEngine.Random) &&
                   methodInfo.Name == "Range";
        }


        public static void LogDebuggingMessages(CodeInstruction instruction, int i)
        {

            // Log each opcode
            UnityEngine.Debug.Log($"Opcode at index {i}: {instruction.opcode}");


            // Log field info.
            if (instruction.operand is FieldInfo fieldInfo2)
            {
                UnityEngine.Debug.Log($"Field Name: {fieldInfo2.Name}, Declaring Type: {fieldInfo2.DeclaringType}");
            }

            // Log random range call.
            if (IsRandomRangeCall(instruction))
            {
                UnityEngine.Debug.Log($"Found the call to UnityEngine.Random.Range at index {i}");
            }

        }

    }
}
