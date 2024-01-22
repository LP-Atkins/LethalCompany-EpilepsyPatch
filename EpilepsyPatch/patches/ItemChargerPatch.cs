using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EpilepsyPatch.patches
{
    [HarmonyPatch]
    public static class ItemChargerPatch
    {
        [HarmonyPatch(typeof(ItemCharger), "chargeItemDelayed", MethodType.Enumerator)]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);
            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];
                //EpilepsyPatchBase.LogDebuggingMessages(instruction, i);

                if (EpilepsyPatchBase.DisableChargerAnimation.Value)
                {
                    if (instructionList[i].opcode == OpCodes.Ldstr && (string)instructionList[i].operand == "zap")
                    {
                        instructionList[i - 2].opcode = OpCodes.Nop;
                        instructionList[i - 1].opcode = OpCodes.Nop;
                        instructionList[i + 0].opcode = OpCodes.Nop;
                        instructionList[i + 1].opcode = OpCodes.Nop;
                        UnityEngine.Debug.Log($"Item Charger animation replaced with NOP");
                    }
                }
            }
            return instructionList.AsEnumerable();
        }
    }
}
