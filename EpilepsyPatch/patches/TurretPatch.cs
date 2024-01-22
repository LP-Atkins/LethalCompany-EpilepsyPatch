using EpilepsyPatch.tools;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EpilepsyPatch.patches
{
    [HarmonyPatch(typeof(Turret))]
    internal class StopTurretAnimator
    {
        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        static void TurretAnimatorPatch(Turret __instance)
        {
            if (EpilepsyPatchBase.TryToHideTurretBullets.Value)
            {
                ParticleSystem.EmissionModule emissionModule = __instance.bulletParticles.emission;
                ParticleSystem.MainModule mainModule = __instance.bulletParticles.main;
                mainModule.startSize = 0.1f;
                mainModule.startColor = Color.clear;
                //__instance.turretAnimator.enabled = false;        //Disabling the animation doesn't stop the muzzle flash and for some reason prevents particle damage?
            }
        }
    }
}
