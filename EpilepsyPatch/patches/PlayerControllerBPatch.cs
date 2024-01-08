using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EpilepsyPatch.patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void adrenalineBoostPatch()
        {
            if (EpilepsyPatchBase.GettingFiredLightDisabled.Value)
            {
                StartOfRound.Instance.shipAnimatorObject.gameObject.GetComponent<Animator>().SetBool("AlarmRinging", value: false);
            }
        }

    }
}
