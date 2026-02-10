using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Unity.IL2CPP.Utils;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using SubspeciesEntry.BepInEx.Buff;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SubspeciesEntry.BepInEx
{
    [BepInPlugin("salmon.subspeciesentry", "Subspecies Entry", "1.0")]
    public class Core : BasePlugin
    {
        public override void Load()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
            TextCore.Load();
            BuffCore.Load();
        }
    }

    public static class TextCore
    {
        public static void Load()
        {
            InitBuffText();
            InitAlmanacText();
        }

        public static void InitBuffText()
        {
            #region 大帝伴侣
            {
                // 流星雨
                ReplaceText.ReplaceBuff(BuffType.UltimateBuff, 8,
                    "流星雨：究极杨桃大帝的攻击间隔降低至0.5秒", "流星雨：究极杨桃大帝的攻击间隔降低至0.5秒；亚种五叶草回旋加速降至0.5秒，吸引范围+50%");
                // 众星之力
                ReplaceText.ReplaceBuff(BuffType.UltimateBuff, 9,
                    "众星之力：究极杨桃大帝的子弹伤害x2（2级时x3），但发射时不会超过3000（2级时无上限）",
                    "众星之力：究极杨桃大帝的子弹伤害x2，伤害上限增至3000；亚种究极五叶草子弹伤害x2。2级时，究极杨桃大帝伤害x3，取消伤害上限；亚种究极五叶草伤害x3，取消储存上限");
                // 斗转星移
                ReplaceText.ReplaceBuff(BuffType.AdvancedBuff, 19,
                    "斗转星移：所有的流星冷却缩短为原来的1/2，且场上每多一个究极杨桃大帝则提升300基础伤害",
                    "斗转星移：所有流星冷却缩短为原来的1/2，所有五叶草储存上限+50%，且场上每多一个究极杨桃大帝则提升300基础伤害，五叶草增加10储存上限");
            }
            #endregion
            #region 金蛋
            {
                // 无尽贪婪
                ReplaceText.ReplaceBuff(BuffType.UltimateBuff, 30,
                    "无尽贪婪：究极超时空玉米的黑洞可以吸引一切子弹",
                    "无尽贪婪：究极超时空玉米的黑洞可以吸引一切子弹；亚种超时空坚果每30秒立即回溯一次并回复2000韧性");
                // 万劫不复
                ReplaceText.ReplaceBuff(BuffType.UltimateBuff, 31,
                    "万劫不复：究极超时空玉米的黑洞吸引子弹的范围大幅增加",
                    "万劫不复：究极超时空玉米的黑洞吸引子弹的范围大幅增加；亚种超时空坚果每次回溯都会净化自身状态");
            }
            #endregion
            #region 血月÷子
            {
                // 金光闪闪
                ReplaceText.ReplaceBuff(BuffType.UltimateBuff, 22,
                    "金光闪闪：太阳神发射子弹时，消耗超过15000部分的阳光的0.5%，使子弹增加消耗阳光数20倍的伤害，亚种月亮神子弹的光照等级增伤×3",
                    "金光闪闪：太阳神发射子弹时，消耗超过15000部分的阳光的0.5%，使子弹增加消耗阳光数20倍的伤害，亚种月亮神子弹的光照等级增伤×3；变种血月神的子弹的光照等级增伤x3，前20级光照等级，每级血月额外提供僵尸的增益提升50%");
                // 人造太阳
                ReplaceText.ReplaceBuff(BuffType.UltimateBuff, 23,
                    "人造太阳：太阳神卷心菜召唤的小太阳伤害x3，亚种月亮神卷心菜召唤的明月治疗量×3",
                    "人造太阳：太阳神卷心菜召唤的小太阳伤害x3，亚种月亮神卷心菜召唤的明月治疗量×3；变种血月神卷心菜召唤的血月持续时间x3，且召唤时间减至5秒");
            }
            #endregion
        }

        public static void InitAlmanacText()
        {
            #region 大帝伴侣
            {
                ReplaceText.ReplaceAlmanac(PlantType.UltimateStar,
                    "子弹伤害×2，伤害上限增至3000。2级，子弹伤害×3，取消伤害上限",
                "子弹伤害x2，伤害上限增至3000；亚种究极五叶草子弹伤害x2。2级时，伤害x3，取消伤害上限；亚种究极五叶草伤害x3，取消储存上限");
                ReplaceText.ReplaceAlmanac(PlantType.UltimateStar,
                    "攻击间隔降低至0.5秒",
                    "攻击间隔降至0.5秒；亚种五叶草回旋加速降至0.5秒，吸引范围+50%");
            }
            #endregion
            #region 金蛋
            {
                // 无尽贪婪
                ReplaceText.ReplaceAlmanac(PlantType.UltimateCorn, "究极黑洞可以吸收绝大多数子弹",
                    "究极黑洞可以吸收绝大多数子弹；亚种超时空坚果每30秒立即回溯一次并回复2000韧性");
                // 万劫不复
                ReplaceText.ReplaceAlmanac(PlantType.UltimateCorn, "究极黑洞吸引子弹的半径翻倍",
                    "究极黑洞吸引子弹的半径翻倍；亚种超时空坚果每次回溯都会净化自身状态");
            }
            #endregion
            #region 血月÷子
            {
                ReplaceText.ReplaceAlmanac(PlantType.UltimateCabbage,
                    "太阳神的子弹会消耗超过15000阳光部分0.5%阳光，使该子弹增加(20×消耗阳光)的伤害；亚种月亮神的子弹的光照等级增伤×3",
                    "太阳神的子弹会消耗超过15000阳光部分0.5%阳光，使该子弹增加（20x消耗阳光）的伤害；亚种月亮神的子弹的光照等级增伤x3；变种血月神的子弹的光照等级增伤x3 ，前20级光照等级，每级血月额外提供僵尸的增益提升50%");
                ReplaceText.ReplaceAlmanac(PlantType.UltimateCabbage,
                    "太阳伤害×3\n月亮回血×3",
                    "太阳伤害x3\n月亮回血x3\n血月持续时间x3，且召唤时间减至5秒");
            }
            #endregion
        }
    }

    #region 修改文本
    public static class ReplaceText
    {
        #region 字段
        public static bool loadAlmanac = false;
        public static bool loadBuff = false;

        public static Dictionary<PlantType, String> AlmanacStrings = new();
        public static List<(PlantType, String, String)> ReplaceAlmanacStrings = new();

        public static Dictionary<(BuffType, int), String> BuffStrings = new();
        public static List<((BuffType, int), String, String)> ReplaceBuffStrings = new();
        #endregion

        /// <summary>
        /// 替换图鉴文本
        /// </summary>
        /// <param name="almanacType">植物类型</param>
        /// <param name="origin">原来的文本</param>
        /// <param name="replace">替换后的文本</param>
        public static void ReplaceAlmanac(PlantType almanacType, String origin, String replace) =>
            ReplaceAlmanacStrings.Add((almanacType, origin, replace));

        /// <summary>
        /// 替换词条文本
        /// </summary>
        /// <param name="plantType">词条对应的植物类型</param>
        /// <param name="type">词条类型</param>
        /// <param name="index">词条ID</param>
        /// <param name="origin">原来的文本</param>
        /// <param name="replace">替换后的文本</param>
        public static void ReplaceBuff(BuffType type, int index, String origin, String replace) =>
            ReplaceBuffStrings.Add(((type, index), origin, replace));

        public static void InitAlmanacText()
        {
            foreach (var (almanacType, origin, replace) in ReplaceAlmanacStrings)
            {
                var almanac = AlmanacPlantMenu.PlantAlmanacData[almanacType];
                var newStr = almanac.info.Replace(origin, replace);
                almanac.info = newStr;
                AlmanacPlantMenu.PlantAlmanacData[almanacType] = almanac;
            }
        }

        public static void InitBuffText()
        {
            foreach (var ((type, index), origin, replace) in ReplaceBuffStrings)
            {
                var oldStr = "";
                switch (type)
                {
                    case BuffType.AdvancedBuff:
                        oldStr = TravelMgr.advancedBuffs[index];
                        break;
                    case BuffType.UltimateBuff:
                        oldStr = TravelMgr.ultimateBuffs[index];
                        break;
                }

                var newStr = oldStr.Replace(origin, replace);
                BuffStrings[(type, index)] = replace;
            }
        }

        public static void InitAlmanac()
        {
            if (!loadAlmanac)
            {
                InitAlmanacText();
                loadAlmanac = true;
            }
            foreach (var (type, str) in AlmanacStrings)
            {
                var almanac = AlmanacPlantMenu.PlantAlmanacData[type];
                almanac.info = str;
                AlmanacPlantMenu.PlantAlmanacData[type] = almanac;
            }
        }

        public static void InitBuff()
        {
            if (!loadBuff)
            {
                InitBuffText();
                loadBuff = true;
            }
            foreach (var ((type, index), str) in BuffStrings)
            {
                switch (type)
                {
                    case BuffType.AdvancedBuff:
                        TravelMgr.advancedBuffs[index] = str;
                        break;
                    case BuffType.UltimateBuff:
                        TravelMgr.ultimateBuffs[index] = str;
                        break;
                }
            }
        }
    }
    #endregion

    #region 基础
    [HarmonyPatch(typeof(AlmanacPlantMenu))]
    public static class AlmanacPlantMenuPatch
    {
        [HarmonyPatch(nameof(AlmanacPlantMenu.InitNameAndInfoFromJson))]
        [HarmonyPostfix]
        public static void PostInitNameAndInfoFromJson()
        {
            ReplaceText.InitAlmanac();
        }
    }
    
    [HarmonyPatch(typeof(TravelLookMenu))]
    public static class TravelLookMenuPatch
    {
        [HarmonyPatch(nameof(TravelLookMenu.Start))]
        [HarmonyPostfix]
        public static void PostStart()
        {
            ReplaceText.InitBuff();
        }
    }

    [HarmonyPatch(typeof(TravelMgr))]
    public static class TravelMgrPatch
    {
        [HarmonyPatch(nameof(TravelMgr.Awake))]
        [HarmonyPostfix]
        public static void PostAwake()
        {
            ReplaceText.InitBuff();
        }
    }
    #endregion

    #region 大帝伴侣
    [HarmonyPatch(typeof(UltimateStarBlover))]
    public static class UltimateStarBloverPatch
    {
        [HarmonyPatch(nameof(UltimateStarBlover.StarsUpdate))]
        [HarmonyPrefix]
        public static void PreStarsUpdate(UltimateStarBlover __instance)
        {
            if (Lawnf.TravelUltimate(8))
                __instance.radius = 1.2f;
            if (Lawnf.TravelUltimate((UltiBuffs)9))
                __instance.maxBullets = int.MaxValue;
            if (__instance.starBullets == null) return;
            if (Lawnf.TravelUltimate((UltiBuffs)8))
            {
                for (int i = 0; i < __instance.starBullets.Length; i++)
                {
                    var star = __instance.starBullets[i];
                    if (star == null || star.IsDestroyed()) continue;
                    star.accelerateTime += Time.deltaTime * 2;
                }
            }
        }

        [HarmonyPatch(nameof(UltimateStarBlover.StarsUpdate))]
        [HarmonyPostfix]
        public static void PostStarsUpdate(UltimateStarBlover __instance)
        {
            if (Lawnf.TravelAdvanced((AdvBuff)19) && !Lawnf.TravelUltimate((UltiBuffs)9))
            {
                for (int i = 0; i < __instance.board.plantStatistics.Count; i++)
                {
                    var plant = __instance.board.plantStatistics[i];
                    if (plant.thePlantType == PlantType.StarPumpkin)
                    {
                        __instance.maxBullets = 360 + 10 * Lawnf.GetPlantCount(PlantType.UltimateStar, __instance.board);
                        return;
                    }
                }
                __instance.maxBullets = 90 + 10 * Lawnf.GetPlantCount(PlantType.UltimateStar, __instance.board);
            }
        }
    }

    [HarmonyPatch(typeof(StarBlover))]
    public static class StarBloverPatch
    {
        [HarmonyPatch(nameof(StarBlover.RemoveNode))]
        [HarmonyPostfix]
        public static void PostRemoveNode(StarBlover __instance, ref Bullet_star starBullet)
        {
            if (__instance.thePlantType == PlantType.UltimateBlover)
            {
                if (Lawnf.TravelUltimate((UltiBuffs)9))
                {
                    starBullet.Damage *= (Lawnf.TravelUltimateLevel(9) == 2) ? 3 : 2;
                }
            }
        }

        [HarmonyPatch(nameof(StarBlover.StarsUpdate))]
        [HarmonyPostfix]
        public static void PostStarsUpdate(StarBlover __instance)
        {
            if (Lawnf.TravelAdvanced((AdvBuff)19))
            {
                for (int i = 0; i < __instance.board.plantStatistics.Count; i++)
                {
                    var plant = __instance.board.plantStatistics[i];
                    if (plant.thePlantType == PlantType.StarPumpkin)
                    {
                        __instance.maxBullets = 120 + 10 * Lawnf.GetPlantCount(PlantType.UltimateStar, __instance.board);
                        return;
                    }
                }
                __instance.maxBullets = 30 + 10 * Lawnf.GetPlantCount(PlantType.UltimateStar, __instance.board);
            }
        }
    }
    #endregion

    #region 金蛋
    [HarmonyPatch(typeof(UltimatePortalNut))]
    public static class UltimatePortalNutPatch
    {
        [HarmonyPatch(nameof(UltimatePortalNut.Revive))]
        [HarmonyPostfix]
        public static void PostRevive(UltimatePortalNut __instance)
        {
            if (__instance == null) return;

            if (Lawnf.TravelUltimate((UltiBuff)31))
            {
                __instance.StartCoroutine(ClearDebuff(__instance));
            }
        }

        public static IEnumerator ClearDebuff(UltimatePortalNut __instance)
        {
            float startTime = Time.time;
            while (Time.time - startTime <= 0.1f)
            {
                try
                {
                    if (__instance == null) yield break;
                    __instance.TryBeActive();
                    if (GameAPP.gameAPP.GetComponent<DelayAction>() == null) yield break;
                    if (GameAPP.gameAPP.GetComponent<DelayAction>().actions == null) yield break;
                    for (int i = GameAPP.gameAPP.GetComponent<DelayAction>().actions.Count - 1; i >= 0; i--)
                    {
                        if (GameAPP.gameAPP.GetComponent<DelayAction>().actions.Count <= 0 || i < 0)
                            break;
                        if (i >= GameAPP.gameAPP.GetComponent<DelayAction>().actions.Count)
                            continue;
                        var action = GameAPP.gameAPP.GetComponent<DelayAction>().actions[i];
                        if (action == null) continue;
                        var target = action.action.Target.TryCast<Bullet_doom_ulti.__c__DisplayClass3_0>();
                        if (target != null && target.plant == __instance)
                        {
                            action.active = false;
                            action.action = null;
                        }
                    }
                    if (Lawnf.GetAllZombies().Count > 0)
                    {
                        foreach (Zombie zombie in Lawnf.GetAllZombies())
                        {
                            if (zombie != null)
                            {
                                if (zombie.theZombieType == ZombieType.UltimateHorse)
                                {
                                    var horse = zombie.GetComponent<UltimateHorse>();
                                    if (horse != null && horse.cursedPlants.Contains(__instance))
                                    {
                                        while (horse.cursedPlants.Contains(__instance))
                                            horse.cursedPlants[horse.cursedPlants.IndexOf(__instance)] = null;
                                        if (!horse.cursedPlants.Contains(__instance))
                                            __instance.SetColor(new Color(1f, 1f, 1f));
                                    }
                                }
                                if (zombie.theZombieType == ZombieType.SuperLadderZombie)
                                {
                                    var ladder = zombie.transform.GetComponent<SuperLadderZombie>();
                                    if (ladder != null && ladder.ladder != null && ladder.ladder.theItemRow == __instance.thePlantRow && ladder.ladder.theItemColumn == __instance.thePlantColumn)
                                    {
                                        ladder.ladder.Die();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (ArgumentOutOfRangeException) { }
                yield return null;
            }
        }
    }

    [HarmonyPatch(typeof(Plant))]
    public static class Plant_UltimatePortalNut_Patch
    {
        [HarmonyPatch(nameof(Plant.Awake))]
        [HarmonyPostfix]
        public static void PostAwake(Plant __instance)
        {
            if (__instance.thePlantType == PlantType.UltimatePortalNut)
            {
                __instance.attributeCountdown = 30f;
            }
        }

        [HarmonyPatch(nameof(Plant.Update))]
        [HarmonyPostfix]
        public static void PostUpdate(Plant __instance)
        {
            if (__instance.thePlantType == PlantType.UltimatePortalNut && __instance.attributeCountdown - Time.deltaTime <= 0f)
            {
                if (Lawnf.TravelUltimate((UltiBuffs)30))
                {
                    __instance.GetComponent<UltimatePortalNut>().Revive();
                    __instance.thePlantHealth += 2000;
                    __instance.thePlantHealth = Mathf.Min(__instance.thePlantMaxHealth, __instance.thePlantHealth);
                    __instance.UpdateText();
                }
                __instance.attributeCountdown = 30f;
            }
        }
    }
    #endregion

    #region 血月
    [HarmonyPatch(typeof(Lunar))]
    public static class LunarPatch
    {
        [HarmonyPatch(nameof(Lunar.SummonUpdate))]
        [HarmonyPrefix]
        public static void PreSummonUpdate(Lunar __instance)
        {
            if (Lawnf.TravelUltimate((UltiBuffs)23))
                __instance.summonTimer -= 2 * Time.deltaTime;
        }

        [HarmonyPatch(nameof(Lunar.Init))]
        [HarmonyPostfix]
        public static void PostInit(Lunar __instance)
        {
            if (Lawnf.TravelUltimate((UltiBuffs)23))
                __instance.lifeTimer *= 3;
        }
    }

    [HarmonyPatch(typeof(Lawnf))]
    public static class LawnfPatch
    {
        [HarmonyPatch(nameof(Lawnf.GetPlantCount), new Type[] { typeof(PlantType), typeof(Board) })]
        [HarmonyPostfix]
        public static void PostGetPlantCount(ref PlantType theSeedType, ref int __result)
        {
            var callByUpdate = false;
            for (int i = 0; i < new StackTrace()?.FrameCount; i++)
            {
                if (new StackTrace()?.GetFrame(i)?.GetMethod()?.Name == "DMD<Lunar::Update>" && new StackTrace()?.GetFrame(i)?.GetMethod()?.GetParameters().Length == 1)
                    callByUpdate = true;
            }
            if (Lawnf.TravelUltimate((UltiBuffs)22) && theSeedType == PlantType.UltimateRedLunar && callByUpdate)
            {
                var maxLevel = Lawnf.GetAllPlants().ToArray().ToList().Where(plant => plant.thePlantType == PlantType.UltimateRedLunar).Max(plant => plant.currentLightLevel);
                maxLevel = Mathf.Min(maxLevel, 20);
                __result += maxLevel;
            }
        }
    }
    #endregion
}
