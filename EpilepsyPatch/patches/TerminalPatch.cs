using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using System.Collections;
using GameNetcodeStuff;
using UnityEngine.UI;

namespace EpilepsyPatch.patches
{

    [HarmonyPatch(typeof(Terminal), "Update")]
    public static class TerminalPatch
    {
        [HarmonyPrefix]
        public static void TerminalCaretPatch(Terminal __instance)
        {
            if (EpilepsyPatchBase.HideTerminalCaret.Value)
            {
                __instance.screenText.caretColor = Color.clear;
            }
        }
    }


}
