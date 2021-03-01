using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;

namespace WeaponIcon
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class BepInExLoader : BasePlugin
    {
        public const string
          MODNAME = "WeaponIconVisible",
          AUTHOR = "Endskill",
          GUID = "dev." + AUTHOR + "." + MODNAME,
          VERSION = "1.1";

        public static ManualLogSource BepInLog;
        public static ConfigEntry<bool> WeaponIcons;

        public override void Load()
        {
            BepInExLoader.BepInLog = this.Log;
            WeaponIcons = Config.Bind("Options", "WeaponIcon", true,
                "Deactivates the only Patch that is needed to show Weapons.");

            Harmony harmony = new Harmony(GUID);
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(PlayerGuiLayer), nameof(PlayerGuiLayer.UpdateGUIElementsVisibility))]
    public class PrepareInjection
    {
        private static System.Random _random = new System.Random();

        [HarmonyPostfix]
        public static void PostFix()
        {
            if (BepInExLoader.WeaponIcons.Value)
            {
                Console.WriteLine("Updating the Icons");
                //Activates the
                var playerLayer = GuiManager.Current.m_playerLayer;

                foreach (var inventorySlot in playerLayer.Inventory.m_inventorySlots)
                {
                    inventorySlot.value.m_selected_icon.enabled = true;
                    //Making 0.01% Chance per Weapon to be BIIIIG
                    if (_random.Next(0, 10000) == 1)
                    {
                        inventorySlot.value.m_selected_icon.size = new Vector2(7600f, 3600f);
                    }
                    else
                    {
                        inventorySlot.value.m_selected_icon.size = new Vector2(145f, 69f);
                    }
                }
            }
        }
    }
}
