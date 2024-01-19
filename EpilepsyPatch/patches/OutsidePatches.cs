using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EpilepsyPatch.tools;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Windows;

namespace EpilepsyPatch.patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class HideTheSunDontPraiseIt
    {
        private static bool hasTriggered = false;

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        static void Postfix(Tools_ListAllGameObjects __instance)
        {
            if (!hasTriggered)
            {
                ReplaceSunMaterial();
            }
        }

        private static void ReplaceSunMaterial()
        {
            GameObject sunObject = GameObject.Find("SunTexture");

            if (sunObject != null)
            {
                Renderer renderer = sunObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material newMaterial = new Material(Shader.Find("Standard"));
                    newMaterial.color = Color.gray;
                    renderer.material = newMaterial;

                    Debug.Log("Material of Sun object replaced with a gray material.");
                }
                else
                {
                    Debug.LogWarning("Renderer component not found on Sun object.");
                }
            }
            else
            {
                Debug.LogWarning("Sun object not found.");
            }
        }
    }

}
