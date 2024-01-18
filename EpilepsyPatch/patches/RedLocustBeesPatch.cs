using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering;
using UnityEngine.VFX;

namespace EpilepsyPatch.patches
{
    [HarmonyPatch(typeof(RedLocustBees), "BeesZap")]
    public static class BeeZapPatch
    {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);


                for (int i = 0; i < instructionList.Count; i++)
                {
                    CodeInstruction instruction = instructionList[i];

                    //EpilepsyPatchBase.LogDebuggingMessages(instruction, i);

                    if (EpilepsyPatchBase.DisableBeeZaps.Value)
                    {
                        if (instructionList[i].opcode == OpCodes.Ldfld && instructionList[i].operand is FieldInfo fieldInfo && fieldInfo.Name == "lightningComponent")
                        {
                            instructionList[i - 1].opcode = OpCodes.Nop;
                            instructionList[i + 0].opcode = OpCodes.Nop;
                            instructionList[i + 1].opcode = OpCodes.Nop;
                            instructionList[i + 2].opcode = OpCodes.Nop;
                            UnityEngine.Debug.Log("Replaced bee lightning call with NOP");
                        }
                    }

                }

                return instructionList.AsEnumerable();
            }
    }
}
