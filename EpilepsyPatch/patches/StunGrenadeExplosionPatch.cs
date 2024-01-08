using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EpilepsyPatch.patches
{
    [HarmonyPatch(typeof(StunGrenadeItem))]
    [HarmonyPatch("ExplodeStunGrenade")]

    public static class StunGrenadeExplosionPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);


            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];

                //Debugging messages, set by constant in 'plugin.cs'
                if (EpilepsyPatchBase.LogDebugMessages)
                {
                    EpilepsyPatchBase.LogDebuggingMessages(instruction, i);
                }

                //Replace the screen filter with NOP.
                if (EpilepsyPatchBase.StunGrenadeExplosionDisabled.Value)
                {
                    if (instructionList[i].opcode == OpCodes.Ldfld && instructionList[i].operand is FieldInfo fieldInfo && fieldInfo.Name == "stunGrenadeExplosion")
                    {
                        instructionList[i - 1].opcode = OpCodes.Nop;
                        instructionList[i + 0].opcode = OpCodes.Nop;
                        instructionList[i + 1].opcode = OpCodes.Nop;
                        instructionList[i + 2].opcode = OpCodes.Nop;
                        instructionList[i + 3].opcode = OpCodes.Nop;
                        instructionList[i + 4].opcode = OpCodes.Nop;
                        instructionList[i + 5].opcode = OpCodes.Nop;
                        instructionList[i + 6].opcode = OpCodes.Nop;
                        instructionList[i + 7].opcode = OpCodes.Nop;
                        UnityEngine.Debug.Log($"Flashbang Explosion replaced with NOP");
                    }
                }

            }

            return instructionList.AsEnumerable();
        }

    }
}
