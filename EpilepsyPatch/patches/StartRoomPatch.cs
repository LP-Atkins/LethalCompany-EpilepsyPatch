using EpilepsyPatch.tools;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Windows;
using UnityEngine;
using GameNetcodeStuff;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using static UnityEngine.Rendering.HighDefinition.WindParameter;

namespace EpilepsyPatch.patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StopEntryRoomFan
    {
        private static bool fanStopped = false;

        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        static void Postfix(Tools_ListAllGameObjects __instance)
        {
            if (!fanStopped)
            {
                DisableIndustrialFanAnimator();
            }

            RemoveHelmet();
            RemoveHelmet2();

            RemoveFog();
            RemoveCriticalHealthWarning();
        }

        private static void DisableIndustrialFanAnimator()
        {
            GameObject industrialFan = GameObject.Find("IndustrialFan");

            if (industrialFan != null)
            {
                Animator animator = industrialFan.GetComponent<Animator>();

                if (animator != null)
                {
                    animator.enabled = false;
                    fanStopped = true;
                    UnityEngine.Debug.Log("Industrial fan found and has been stopped");
                }
                else
                {
                    //UnityEngine.Debug.LogWarning("Animator component not found on IndustrialFan.");
                }
            }
            else
            {
                //UnityEngine.Debug.LogWarning("IndustrialFan object not found.");
            }
        }


        private static void RemoveCriticalHealthWarning()
        {
            if (EpilepsyPatchBase.DisableCriticalHealthMessage.Value)
            {
                GameObject CriticalInjury = GameObject.Find("CriticalInjury");
                if (CriticalInjury != null)
                {
                    CriticalInjury.SetActive(false);
                }
            }
        }

        //Removes entire helmet.
        private static void RemoveHelmet()
        {
            //PlayerHUDHelmetMode.ScavengerHelmet.Plane
            if (EpilepsyPatchBase.DisableFPVHelmet.Value)
            {
                GameObject PlayerHUDHelmetModel = GameObject.Find("ScavengerHelmet");

                if (PlayerHUDHelmetModel != null)
                {
                    PlayerHUDHelmetModel.SetActive(false);
                }
            }
        }

        //Removes only the glass from the helmet.
        public static void RemoveHelmet2()
        {
            if (EpilepsyPatchBase.DisableFPVHelmetGlass.Value)
            {
                GameObject scavengerHelmet = GameObject.Find("ScavengerHelmet");
                if (scavengerHelmet != null)
                {
                    MeshRenderer meshRenderer = scavengerHelmet.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        Material[] materials = meshRenderer.materials;
                        if (materials.Length >= 3)
                        {
                            Material[] updatedMaterials = new Material[materials.Length - 1];
                            System.Array.Copy(materials, updatedMaterials, 2);
                            System.Array.Copy(materials, 3, updatedMaterials, 2, materials.Length - 3);

                            // Assign the updated materials array back to the MeshRenderer
                            meshRenderer.materials = updatedMaterials;

                            //Optional.
                            UnityEngine.Object.Destroy(materials[2]);
                        }
                        //else
                        //{
                        //    Debug.LogError("The materials array does not have enough materials.");
                        //}
                    }
                }
            }
        }




        private static void RemoveFog()
        {
            Fog fog;
            GameObject VolumeMain = GameObject.Find("VolumeMain");

            if (VolumeMain != null)
            {
                //Disable all fog.
                if (EpilepsyPatchBase.DisableFog.Value)
                {
                    VolumeMain.SetActive(false);
                }

                //Disable only volumetric fog (3D fog).
                if (EpilepsyPatchBase.DisableVolumetricFog.Value)
                {
                    Volume volumeComponent = VolumeMain.GetComponent<Volume>();
                    volumeComponent.sharedProfile.TryGet<Fog>(out fog);

                    fog.enableVolumetricFog.value = false;
                }
            }
        }

        //Finds a specified VolumeProfile
        private static VolumeProfile FindVolumeProfile(string profileName)
        {
            VolumeProfile[] allVolumeProfiles = UnityEngine.Object.FindObjectsOfType<VolumeProfile>();
            foreach (VolumeProfile volumeProfile in allVolumeProfiles)
            {
                if (volumeProfile.name == profileName)
                {
                    return volumeProfile;
                }
            }

            return null;
        }



        [HarmonyPrefix]
        [HarmonyPatch("AutoSaveShipData")]
        private static void resetFanDisabled()
        {
            fanStopped = false;
            UnityEngine.Debug.Log("Resetting fan stopped trigger.");
        }


        [HarmonyPatch(typeof(RoundManager))]
        [HarmonyPrefix]
        [HarmonyPatch("LoadNewLevel")]
        private static void resetFanDisabled2()
        {
            fanStopped = false;
            UnityEngine.Debug.Log("Resetting fan stopped trigger.");
        }

    }
}
