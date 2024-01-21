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

        [HarmonyPrefix]
        [HarmonyPatch("AutoSaveShipData")]
        private static void resetFanDisabled()
        {
            fanStopped = false;
            UnityEngine.Debug.Log("Resetting fan stopped trigger.");
        }
    }
}
