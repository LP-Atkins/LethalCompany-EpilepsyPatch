using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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
        static void shipLightsPatch()
        {
            if (EpilepsyPatchBase.GettingFiredLightDisabled.Value)
            {
                StartOfRound.Instance.shipAnimatorObject.gameObject.GetComponent<Animator>().SetBool("AlarmRinging", value: false);
            }

            if (EpilepsyPatchBase.ForceShipLightsOn.Value)
            {
                StartOfRound.Instance.shipRoomLights.gameObject.GetComponent<ShipLights>().areLightsOn = false;     //this doesn't set the state it just keeps track of it.
            }
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void TeleporterBeamUpPatch(ref PlayerControllerB __instance)
        {
            if (EpilepsyPatchBase.DisableBeamUpParticles.Value)
            {
                __instance.beamUpParticle.Stop();
                __instance.beamOutParticle.Stop();
                __instance.beamOutBuildupParticle.Stop();
            }
        }
    }


    [HarmonyPatch(typeof(PlayerControllerB), "ChangeHelmetLight")]
    public static class SwitchFlashlightSpamPatch
    {
        private static bool canTurnOnFlashlight = true;

        [HarmonyPrefix]
        public static bool FLSpamPrefix(PlayerControllerB __instance)
        {
            if (!canTurnOnFlashlight && EpilepsyPatchBase.PreventFlashlightSpam.Value)
            {
                //UnityEngine.Debug.Log("Network torch is being spammed, discarding state change.");
                return false;
            }

            //Start the cooldown coroutine.
            __instance.StartCoroutine(FlashlightCooldown());

            return true;
        }

        private static IEnumerator FlashlightCooldown()
        {
            canTurnOnFlashlight = false;
            yield return new WaitForSeconds(EpilepsyPatchBase.FlashlightSpamCooldown.Value);  //Cooldown is in seconds.
            canTurnOnFlashlight = true;
        }

    }
}
