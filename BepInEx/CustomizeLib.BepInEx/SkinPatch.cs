using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomizeLib.BepInEx
{
    [HarmonyPatch(typeof(NutFume))]
    public static class NutFumePatch
    {
        [HarmonyPatch(nameof(NutFume.ReplaceSprite))]
        [HarmonyPrefix]
        public static void PreReplaceSprite(NutFume __instance)
        {
            if (SkinMgr.IsPlantSkinEnable(__instance.thePlantType) && __instance.changes.Count <= 0)
            {
                __instance.changes.Add(__instance.transform.FindChild("FumeShroom_head").gameObject);
                __instance.changes.Add(__instance.transform.FindChild("FumeShroom_body").gameObject);
            }
        }
    }

    [HarmonyPatch(typeof(Bullet_pea_bombCherry))]
    public static class Bullet_pea_bombCherryPatch
    {
        [HarmonyPatch(nameof(Bullet_pea_bombCherry.HitZombie))]
        [HarmonyPrefix]
        public static void PreHitZombie(Bullet_pea_threeCherry __instance)
        {
            if (Regex.IsMatch(__instance.gameObject.name, @"Bullet_(\d+)"))
            {
                __instance.bombPrefab = Resources.Load<GameObject>("items/timebomb/TimeBomb");
            }
        }
    }

    [HarmonyPatch(typeof(Bullet_pea_threeCherry))]
    public static class Bullet_pea_threeCherryPatch
    {
        [HarmonyPatch(nameof(Bullet_pea_threeCherry.HitZombie))]
        [HarmonyPrefix]
        public static void PreHitZombie(Bullet_pea_threeCherry __instance)
        {
            if (Regex.IsMatch(__instance.gameObject.name, @"Bullet_(\d+)"))
            {
                __instance.bombPrefab = Resources.Load<GameObject>("items/timebomb/TimeBomb");
            }
        }
    }
}
