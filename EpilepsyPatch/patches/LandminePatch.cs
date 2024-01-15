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
    //This hides explosions from landmines and lightning.
    [HarmonyPatch(typeof(Landmine), "SpawnExplosion")]
    public static class ExplosionPatch
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);


            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];

                //EpilepsyPatchBase.LogDebuggingMessages(instruction, i);


                if (EpilepsyPatchBase.HideLightningExplosions.Value)
                {
                    if (instructionList[i].opcode == OpCodes.Ldfld && instructionList[i].operand is FieldInfo fieldInfo && fieldInfo.Name == "explosionPrefab")
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
                        instructionList[i + 8].opcode = OpCodes.Nop;
                        instructionList[i + 9].opcode = OpCodes.Nop;
                        instructionList[i + 10].opcode = OpCodes.Nop;
                        instructionList[i + 11].opcode = OpCodes.Nop;
                        UnityEngine.Debug.Log($"Explosion animator replaced with NOP");
                    }
                }

            }
            return instructionList.AsEnumerable();
        }

    }
}
