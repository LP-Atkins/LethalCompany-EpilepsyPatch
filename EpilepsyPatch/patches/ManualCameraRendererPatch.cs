using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EpilepsyPatch.patches
{
    [HarmonyPatch(typeof(ManualCameraRenderer))]
    internal class ManualCameraRendererPatch    //This is a stupid way of doing it, but it seems to get the job done.
    {
        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        static void Postfix(ManualCameraRenderer __instance)
        {
            Animator mapCameraAnimator = __instance.mapCameraAnimator;

            if (mapCameraAnimator != null && EpilepsyPatchBase.DisablePlayerMonitorBlink.Value)
            {
                //mapCameraAnimator.speed = 0;          //This left a green line on the screen of the first frame of the animation.
                mapCameraAnimator.enabled = false;
            }
        }
    }
}
