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
    [HarmonyPatch("StunExplosion")]

    public static class StunGrenadePatch
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
                if (EpilepsyPatchBase.StunGrenadeFilterDisabled.Value)
                {
                    if (instructionList[i].opcode == OpCodes.Ldfld && instructionList[i].operand is FieldInfo fieldInfo && fieldInfo.Name == "flashbangScreenFilter")
                    {
                        instructionList[i - 1].opcode = OpCodes.Nop;
                        instructionList[i + 0].opcode = OpCodes.Nop;
                        instructionList[i + 1].opcode = OpCodes.Nop;
                        instructionList[i + 2].opcode = OpCodes.Nop;
                        UnityEngine.Debug.Log($"Flashbang filter replaced with NOP");
                    }
                }

                }
            return instructionList.AsEnumerable();
        }

    }
}
