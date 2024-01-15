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
    //This hides the lightning bolt.
    [HarmonyPatch(typeof(StormyWeather), "LightningStrike")]
    public static class LightningPatch
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);

            int explosionEffectParticleCounter = 0;

            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];

                //EpilepsyPatchBase.LogDebuggingMessages(instruction, i);


                //Kill lightning.
                if (EpilepsyPatchBase.HideLightningStrikes.Value)
                {
                    if (instructionList[i].opcode == OpCodes.Stfld && instructionList[i].operand is FieldInfo fieldInfo && fieldInfo.Name == "AutomaticModeSeconds")
                    {
                        instructionList[i - 2].opcode = OpCodes.Nop;
                        instructionList[i - 1].opcode = OpCodes.Nop;
                        instructionList[i + 0].opcode = OpCodes.Nop;
                        UnityEngine.Debug.Log($"Lightning script replaced with NOP");
                    }
                }


                //kill lightning explosion particle.
                if (EpilepsyPatchBase.HideLightningExplosions.Value)
                {

                    if (instructionList[i].opcode == OpCodes.Ldfld && instructionList[i].operand is FieldInfo fieldInfo && fieldInfo.Name == "explosionEffectParticle")
                    {
                        if (explosionEffectParticleCounter == 1) //We only want to replace the second instance we come across, which is the 'play' trigger.
                        {
                            instructionList[i - 1].opcode = OpCodes.Nop;
                            instructionList[i + 0].opcode = OpCodes.Nop;
                            instructionList[i + 1].opcode = OpCodes.Nop;
                            UnityEngine.Debug.Log($"Lightning explosion trigger replaced with NOP");
                            
                        }
                        else
                        {
                            explosionEffectParticleCounter++;
                        }
                    }


                }




            }
            return instructionList.AsEnumerable();
        }

    }
}