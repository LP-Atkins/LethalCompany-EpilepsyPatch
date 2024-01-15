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
    public static class ShipTeleporterPatch
    {
        [HarmonyPatch(typeof(ShipTeleporter), "beamOutPlayer", MethodType.Enumerator)]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);
            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];
                EpilepsyPatchBase.LogDebuggingMessages(instruction, i);
            }
            return instructionList.AsEnumerable();
        }
    }
}
