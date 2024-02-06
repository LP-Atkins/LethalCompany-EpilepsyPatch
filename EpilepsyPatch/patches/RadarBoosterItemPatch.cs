using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EpilepsyPatch.patches
{
    [HarmonyPatch]
    public static class RadarBoosterPatch
    {
        [HarmonyPatch(typeof(RadarBoosterItem), "EnableRadarBooster")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);
            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];
                //EpilepsyPatchBase.LogDebuggingMessages(instruction, i);

                if (EpilepsyPatchBase.DisableRadarBoosterAnimation.Value)
                {
                    if (instructionList[i].opcode == OpCodes.Ldstr && (string)instructionList[i].operand == "on")
                    {
                        instructionList[i].operand = "off";
                        UnityEngine.Debug.Log($"Radar booster on animation replaced with off");
                    }
                }
            }
            return instructionList.AsEnumerable();
        }
    }

    [HarmonyPatch(typeof(RadarBoosterItem), "Flash")]
    public static class RadarBoosterPatch2
    {
        [HarmonyPrefix]
        static bool RemoveFlash()
        {
            if (EpilepsyPatchBase.DisableRadarBoosterFlash.Value)
            {
                return false;
            }
            return true;
        }
    }
}
