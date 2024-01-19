using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EpilepsyPatch.patches
{
    //There are two components to the flashlight, the actual light and the bulb glow.
    [HarmonyPatch(typeof(FlashlightItem), "SwitchFlashlight")]
    public static class FlashlightSpamPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);

            int flashlightBulbCounter = 0;

            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];

                //EpilepsyPatchBase.LogDebuggingMessages(instruction, i);

                if (EpilepsyPatchBase.PreventFlashlightSpam.Value)
                {
                    if (instructionList[i].opcode == OpCodes.Ldfld && instructionList[i].operand is FieldInfo fieldInfo && fieldInfo.Name == "flashlightBulb")
                    {
                        if (flashlightBulbCounter == 1)
                        {
                            instructionList[i - 1].opcode = OpCodes.Nop;
                            instructionList[i + 0].opcode = OpCodes.Nop;
                            instructionList[i + 1].opcode = OpCodes.Nop;
                            instructionList[i + 2].opcode = OpCodes.Nop;
                            UnityEngine.Debug.Log($"Flashlight bulb replaced with NOP");
                        }
                        else
                        {
                            flashlightBulbCounter++;
                        }
                    }
                }

            }

            return instructionList.AsEnumerable();
        }

    }




    //You can adjust the cooldown of items using the base class.... however this only changes the cooldown on the local client....
    [HarmonyPatch(typeof(FlashlightItem), "Update")]
    public static class SwitchFlashlightPatch
    {
        [HarmonyPrefix]
        public static void Prefix(FlashlightItem __instance)
        {
            __instance.useCooldown = 0.5f;
        }
    }


}
