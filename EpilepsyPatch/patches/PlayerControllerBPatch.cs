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
using UnityEngine.Rendering.HighDefinition;

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

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        static void MakeFogStill()
        {
            if (EpilepsyPatchBase.DisableFogMovement.Value)
            {
                GameObject brightDay = GameObject.Find("BrightDay");

                if (brightDay != null)
                {
                    // Get the LocalVolumetricFog component in the children of the "BrightDay" GameObject
                    LocalVolumetricFog localVolumetricFog = brightDay.GetComponentInChildren<LocalVolumetricFog>();

                    // Check if the direct reference didn't work, try with the name
                    if (localVolumetricFog == null)
                    {
                        localVolumetricFog = brightDay.GetComponentInChildren<LocalVolumetricFog>(true);
                    }

                    if (localVolumetricFog != null)
                    {
                        // Access the LocalVolumetricFogArtistParameters directly and set the textureScrollingSpeed
                        localVolumetricFog.parameters.textureScrollingSpeed = Vector3.zero;

                        //Debug.Log("Set LocalVolumetricFogArtistParameters.textureScrollingSpeed to (0, 0, 0)");
                    }
                    else
                    {
                        //Debug.LogError("LocalVolumetricFog component not found on BrightDay GameObject or its children");
                    }
                }
                else
                {
                    //Debug.LogError("BrightDay GameObject not found");
                }
            }
        }

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        static void DustStorm()
        {
            if (EpilepsyPatchBase.DisableFogMovement.Value)
            {
                GameObject TimeAndWeather = GameObject.Find("TimeAndWeather");
                if (TimeAndWeather != null)
                {
                    GameObject DustStorm = TimeAndWeather.transform.Find("DustStorm")?.gameObject;
                    if (DustStorm != null)
                    {
                        LocalVolumetricFog dustVolumetricFog = DustStorm.GetComponent<LocalVolumetricFog>();
                        if (dustVolumetricFog != null)
                        {
                            dustVolumetricFog.enabled = false;
                            //UnityEngine.Debug.Log("Disabling the dust storm");
                        }
                        else
                        {
                            //UnityEngine.Debug.Log("unable to locate the dust volume");
                        }
                    }
                    else
                    {
                        //UnityEngine.Debug.Log("Unable to locate DustStorm");
                    }
                }
                else
                {
                    //UnityEngine.Debug.Log("Unable to locate TimeAndWeather");
                }
            }
        }

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        static void DisableFoggy()
        {
            if (EpilepsyPatchBase.DisableFoggyModifier.Value)
            {
                GameObject TimeAndWeather = GameObject.Find("TimeAndWeather");
                if (TimeAndWeather != null)
                {
                    GameObject Foggy = TimeAndWeather.transform.Find("Foggy")?.gameObject;
                    if (Foggy != null)
                    {
                        Foggy.SetActive(false);
                    }
                }
            }
        }


        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        static void KillInsanityFilter()
        {
            if (EpilepsyPatchBase.DisableFearScreenFilter.Value)
            {
                GameObject InsanityFilter = GameObject.Find("InsanityFilter");
                if ( InsanityFilter != null )
                {
                    InsanityFilter.SetActive(false);
                }
            }

        }

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        static void KillFlashFilter()
        {
            if (EpilepsyPatchBase.StunGrenadeFilterDisabled.Value)
            {
                GameObject FlashbangFilter = GameObject.Find("FlashbangFilter");
                if (FlashbangFilter != null)
                {
                    FlashbangFilter.SetActive(false);
                }
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
