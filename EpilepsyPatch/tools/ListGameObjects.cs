using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

namespace EpilepsyPatch.tools
{

    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class Tools_ListAllGameObjects
    {
        private static bool hasTriggered1 = false;
        private static bool hasTriggered2 = false;

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        static void Postfix(Tools_ListAllGameObjects __instance)
        {
            // Check if the ObjectLogTrigger action is triggered
            if (Keyboard.current != null && Keyboard.current.f10Key.wasPressedThisFrame && !hasTriggered1)
            {
                //ListGameObjects(__instance);
                hasTriggered1 = true;
            }

            if (Keyboard.current != null && Keyboard.current.f11Key.wasPressedThisFrame && !hasTriggered2)
            {
                //ListRenderingGameObjects(__instance);
                LogGOinformation(__instance, "VolumeMain");
                hasTriggered2 = true;
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

        //List of Child GameObjects under 'Rendering':
            //VolumeMain                : Volumetric fog
            //CustomPass                : Apparently this is Zeekers custom shader and edge detection.   
            //PlayerHUDHelmetModel      : Facemask 3D model for FPV
            //ScanSphere                : No idea
            //InsanityFilter            : Fear effect filter
            //FlashbangFilter           : Flashbang filter
            //UnderwaterFilter          : Well what do you think this is?
            //StarsSphere               : No idea
            //DrunknessFilter           : Inhalent filter???
            //BlackSkyVolume            : No idea.
        private static void ListRenderingGameObjects(Tools_ListAllGameObjects toolsInstance)
        {
            GameObject renderingGameObject = GameObject.Find("Plane");

            if (renderingGameObject != null)
            {
                Debug.Log("List of Child GameObjects under 'Rendering':");

                for (int i = 0; i < renderingGameObject.transform.childCount; i++)
                {
                    Transform childTransform = renderingGameObject.transform.GetChild(i);
                    Debug.Log(childTransform.gameObject.name);
                }
            }
            else
            {
                Debug.LogError("No 'Rendering' GameObject found in the scene");
            }
        }



        private static void LogGOinformation(Tools_ListAllGameObjects toolsInstance, string gObject)
        {
            GameObject volumeMainGameObject = GameObject.Find(gObject);

            if (volumeMainGameObject != null)
            {
                Component[] components = volumeMainGameObject.GetComponents<Component>();

                foreach (Component component in components)
                {
                    Debug.Log($"Parameters of {component.GetType().Name} on {gObject} GameObject:");

                    System.Type type = component.GetType();
                    System.Reflection.FieldInfo[] fields = type.GetFields();
                    System.Reflection.PropertyInfo[] properties = type.GetProperties();

                    foreach (var field in fields)
                    {
                        Debug.Log($"{field.Name}: {field.GetValue(component)}");
                    }

                    foreach (var property in properties)
                    {
                        Debug.Log($"{property.Name}: {property.GetValue(component)}");
                    }
                }
            }
            else
            {
                Debug.LogError($"No {gObject} GameObject found in the scene");
            }
        }




    }

}