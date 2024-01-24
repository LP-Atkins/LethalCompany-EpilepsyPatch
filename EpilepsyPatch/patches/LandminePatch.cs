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
    [HarmonyPatch(typeof(Landmine), "SetOffMineAnimation")]
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
                    if (instructionList[i].opcode == OpCodes.Ldstr && (string)instructionList[i].operand == "detonate")
                    {
                        instructionList[i - 2].opcode = OpCodes.Nop;
                        instructionList[i - 1].opcode = OpCodes.Nop;
                        instructionList[i + 0].opcode = OpCodes.Nop;
                        instructionList[i + 1].opcode = OpCodes.Nop;
                        UnityEngine.Debug.Log($"Landmine detonation animation trigger replaced with NOP");
                    }
                }

            }
            return instructionList.AsEnumerable();
        }

    }
}
