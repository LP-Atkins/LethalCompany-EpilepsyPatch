using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EpilepsyPatch.tools
{

    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class Tools_ListAllGameObjects
    {
        private static bool hasTriggered = false;

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        static void Postfix(Tools_ListAllGameObjects __instance)
        {
            // Check if the ObjectLogTrigger action is triggered
            if (Keyboard.current != null && Keyboard.current.f10Key.wasPressedThisFrame && !hasTriggered)
            {
                ListGameObjects(__instance);
                hasTriggered = true;
            }
        }

        // Custom method to list all GameObjects
        private static void ListGameObjects(Tools_ListAllGameObjects toolsInstance)
        {
            GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

            Debug.Log("List of GameObjects:");

            foreach (GameObject go in allGameObjects)
            {
                Debug.Log(go.name);
            }
        }
    }

}