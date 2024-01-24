using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GameNetcodeStuff;
using UnityEngine;
using System.Runtime.Remoting;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace EpilepsyPatch.patches
{
    [HarmonyPatch(typeof(HUDManager))]
    [HarmonyPatch("PingScan_performed")]
    public static class PingScan_performedPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);


            for (int i = 0; i < instructionList.Count; i++)
            {
                //CodeInstruction instruction = instructionList[i];
                //EpilepsyPatchBase.LogDebuggingMessages(instruction, i);

                if (EpilepsyPatchBase.ScanBlueFlashDisabled.Value)
                {
                    if (instructionList[i].opcode == OpCodes.Ldstr && (string)instructionList[i].operand == "scan")
                    {
                        instructionList[i - 2].opcode = OpCodes.Nop;
                        instructionList[i - 1].opcode = OpCodes.Nop;
                        instructionList[i + 0].opcode = OpCodes.Nop;
                        instructionList[i + 1].opcode = OpCodes.Nop;
                        UnityEngine.Debug.Log($"Scan animation replaced with NOP");
                    }
                }

            }

            return instructionList.AsEnumerable();
        }

    }


    [HarmonyPatch(typeof(HUDManager))]
    [HarmonyPatch("DisplayGlobalNotification")]
    public static class DisplayGlobalNotification_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);


            for (int i = 0; i < instructionList.Count; i++)
            {
                //CodeInstruction instruction = instructionList[i];
                //EpilepsyPatchBase.LogDebuggingMessages(instruction, i);

                if (EpilepsyPatchBase.DisableGlobalNotifications.Value)
                {
                    if (instructionList[i].opcode == OpCodes.Ldstr && (string)instructionList[i].operand == "TriggerNotif")
                    {
                        instructionList[i - 2].opcode = OpCodes.Nop;
                        instructionList[i - 1].opcode = OpCodes.Nop;
                        instructionList[i + 0].opcode = OpCodes.Nop;
                        instructionList[i + 1].opcode = OpCodes.Nop;
                        UnityEngine.Debug.Log($"Notification animation replaced with NOP");
                    }
                }

            }

            return instructionList.AsEnumerable();
        }

    }



    [HarmonyPatch(typeof(HUDManager))]
    [HarmonyPatch("RadiationWarningHUD")]
    public static class RadiationWarningHUD_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);


            for (int i = 0; i < instructionList.Count; i++)
            {
                //CodeInstruction instruction = instructionList[i];
                //EpilepsyPatchBase.LogDebuggingMessages(instruction, i);

                if (EpilepsyPatchBase.DisableRadiationWarning.Value)
                {
                    if (instructionList[i].opcode == OpCodes.Ldstr && (string)instructionList[i].operand == "RadiationWarning")
                    {
                        instructionList[i - 2].opcode = OpCodes.Nop;
                        instructionList[i - 1].opcode = OpCodes.Nop;
                        instructionList[i + 0].opcode = OpCodes.Nop;
                        instructionList[i + 1].opcode = OpCodes.Nop;
                        UnityEngine.Debug.Log($"Radiation warning replaced with NOP");
                    }
                }

            }

            return instructionList.AsEnumerable();
        }

    }



    [HarmonyPatch(typeof(HUDManager))]
    [HarmonyPatch("DisplayTip")]
    public static class DisplayTip_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);


            for (int i = 0; i < instructionList.Count; i++)
            {
                //CodeInstruction instruction = instructionList[i];
                //EpilepsyPatchBase.LogDebuggingMessages(instruction, i);

                if (EpilepsyPatchBase.ReplaceWarningWithHint.Value)
                {
                    if (instructionList[i].opcode == OpCodes.Ldstr && (string)instructionList[i].operand == "TriggerWarning")
                    {
                        instructionList[i + 0].operand = "TriggerHint";
                        UnityEngine.Debug.Log($"TriggerWarning replaced with TriggerHint");
                    }
                }

            }

            return instructionList.AsEnumerable();
        }

    }


    [HarmonyPatch(typeof(HUDManager))]
    internal class insanityFilterPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        //static void shipLightsPatch(ref Volume ___insanityScreenFilter, ref Volume ___flashbangScreenFilter)
        static void shipLightsPatch(HUDManager __instance)
        {
            if (EpilepsyPatchBase.DisableFearScreenFilter.Value)
            {
                //___insanityScreenFilter.weight = 0f;
                __instance.insanityScreenFilter.weight = 0f;

            }

            //if (EpilepsyPatchBase.DisableRadarBoosterAnimation.Value)     //for some unknown reason, doesn't not work with the radar booster.
            //{
                //___flashbangScreenFilter.weight = 0f;
            //    __instance.flashbangScreenFilter.weight = 0f;
            //    __instance.flashFilter = 0;
            //    UnityEngine.Debug.Log("Filter set to 0");
            //}
        }

    }

}