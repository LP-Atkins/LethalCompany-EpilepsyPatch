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
using System.Collections;

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
        }

    }


    [HarmonyPatch(typeof(HUDManager))]
    internal class ToolTipFlashPatch
    {
        [HarmonyPatch("DisplayTip")]
        [HarmonyPostfix]
        static void PostFix()
        {
            //Yeah this is a stupid way to do it, but you explain to me how to edit an animator and I will update it to suit.
            if (EpilepsyPatchBase.TryToStopTooltipFlash.Value)
            {
                HUDManager.Instance.tipsPanelAnimator.speed = 0.1f;
                HUDManager.Instance.StartCoroutine(PauseAnimation());
                HUDManager.Instance.StartCoroutine(FastForwardAnimation());
            }
        }

        private static IEnumerator PauseAnimation()
        {
            yield return new WaitForSeconds(0.9f);
            HUDManager.Instance.tipsPanelAnimator.speed = 0;
        }

        private static IEnumerator FastForwardAnimation()
        {
            yield return new WaitForSeconds(6f);
            HUDManager.Instance.tipsPanelAnimator.speed = 10000;
        }
    }

}