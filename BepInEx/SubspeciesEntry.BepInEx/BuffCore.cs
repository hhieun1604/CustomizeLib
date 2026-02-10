using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SubspeciesEntry.BepInEx.Buff
{
    public static class BuffCore
    {
        public static Dictionary<PlantType, List<AdvBuff>> CustomPlantAdvBuff = new();

        public static void Load()
        {
            //InitBuff();
        }
        
        public static void InitPlant()
        {
            /*#region 🤺仙
            {
                var ab = Tools.GetAssetBundle(Assembly.GetExecutingAssembly(), "abyssswordstar");
                var prefab = ab.GetAsset<GameObject>("Prefab");
                prefab.AddComponent<AbyssSwordStar>();
                GameAPP.resourcesManager.plantPrefabs[PlantType.AbyssSwordStar] = prefab;
            }
            #endregion*/
        }

        /// <summary>
        /// 初始化词条
        /// </summary>
        public static void InitBuff()
        {
            #region 🤺仙
            {
                RegisterCustomAdvancedBuff(PlantType.AbyssSwordStar, PlantType.AbyssSwordStar, "行侠仗义：究极剑仙杨桃攻击间隔减少1/3，飞剑伤害和数量x10",
                    () => Lawnf.TravelAdvanced((AdvBuff)57) && Board.Instance.ObjectExist<AbyssSwordStar>());
                RegisterCustomAdvancedBuff(PlantType.AbyssSwordStar, PlantType.AbyssSwordStar, "醉酒当歌：究极剑仙杨桃攻击间隔减少1/3，近战伤害x3",
                    () => Lawnf.TravelAdvanced((AdvBuff)57) && Board.Instance.ObjectExist<AbyssSwordStar>());
            }
            #endregion
        }

        /// <summary>
        /// 注册自定义高级词条
        /// </summary>
        /// <param name="plantType">词条对应植物</param>
        /// <param name="icon">词条界面显示植物</param>
        /// <param name="text">词条描述</param>
        /// <param name="unlock">解锁条件</param>
        /// <param name="cost">价格</param>
        public static void RegisterCustomAdvancedBuff(PlantType plantType, PlantType icon, String text, Func<bool> unlock, int cost = 5000)
        {
            var id = BuffRegister.RegisterCustomAdvancedBuff(icon, text, unlock, cost);
            if (CustomPlantAdvBuff.ContainsKey(plantType))
                CustomPlantAdvBuff[plantType].Add(id);
            else
                CustomPlantAdvBuff.Add(plantType, new List<AdvBuff> { id });
        }

        /// <summary>
        /// 获取植物对应的高级词条
        /// </summary>
        /// <param name="plantType">植物类型</param>
        /// <returns>高级词条</returns>
        public static List<AdvBuff> GetAdvBuffsByPlantType(PlantType plantType) => CustomPlantAdvBuff[plantType];

        /// <summary>
        /// 获取植物是否获取该植物的指定词条
        /// </summary>
        /// <param name="plantType">植物类型</param>
        /// <param name="index">索引</param>
        /// <returns>是否解锁</returns>
        public static bool GetPlantAdvBuffUnlock(PlantType plantType, int index) => Lawnf.TravelAdvanced(GetAdvBuffsByPlantType(plantType)[index]);
    }

    #region 注册词条
    public static class BuffRegister
    {
        public static Dictionary<int, (PlantType, string, Func<bool>, int)> CustomAdvancedBuffs { get; set; } = [];

        /// <summary>
        /// 注册自定义高级词条
        /// </summary>
        /// <param name="icon">词条界面显示植物</param>
        /// <param name="text">词条描述</param>
        /// <param name="unlock">解锁条件</param>
        /// <param name="cost">价格</param>
        public static AdvBuff RegisterCustomAdvancedBuff(PlantType icon, String text, Func<bool> unlock, int cost = 5000)
        {
            int i = TravelMgr.advancedBuffs.Count;
            TravelMgr.advancedBuffs.Add(i, text);
            CustomAdvancedBuffs.Add(i, (icon, text, unlock, cost));
            return (AdvBuff)i;
        }

        public static bool ObjectExist<T>(this Board board) => board.GetComponentsInChildren<T>().Count > 0;
    }
    #endregion

    #region 基础
    [HarmonyPatch(typeof(TravelMgr))]
    public static class TravelMgrPatch
    {
        [HarmonyPatch(nameof(TravelMgr.Awake))]
        [HarmonyPriority(-1900)]
        [HarmonyPrefix]
        public static void PreAwake(TravelMgr __instance)
        {
            var newAdv = new bool[__instance.advancedUpgrades.Length + BuffRegister.CustomAdvancedBuffs.Count];
            Array.Copy(__instance.advancedUpgrades, newAdv, __instance.advancedUpgrades.Length);
            __instance.advancedUpgrades = newAdv;
        }

        [HarmonyPatch(nameof(TravelMgr.GetAdvancedText))]
        [HarmonyPostfix]
        public static void PostGetAdvancedText(int index, ref string __result)
        {
            if (BuffRegister.CustomAdvancedBuffs.ContainsKey(index))
                __result = BuffRegister.CustomAdvancedBuffs[index].Item2;
        }

        [HarmonyPatch(nameof(TravelMgr.GetAdvancedBuffPool))]
        [HarmonyPostfix]
        public static void PostGetAdvancedBuffPool(ref Il2CppSystem.Collections.Generic.List<int> __result)
        {
            for (int i = __result.Count - 1; i >= 0; i--)
                if (BuffRegister.CustomAdvancedBuffs.ContainsKey(__result[i]) && !BuffRegister.CustomAdvancedBuffs[__result[i]].Item3.Invoke())
                    __result.Remove(__result[i]);
        }

        [HarmonyPatch(nameof(TravelMgr.GetPlantTypeByAdvBuff))]
        [HarmonyPostfix]
        public static void PostGetPlantTypeByAdvBuff(ref int index, ref PlantType __result)
        {
            if (BuffRegister.CustomAdvancedBuffs.ContainsKey(index))
                __result = BuffRegister.CustomAdvancedBuffs[index].Item1;
        }
    }

    [HarmonyPatch(typeof(TravelStore))]
    public static class TravelStorePatch
    {
        [HarmonyPatch(nameof(TravelStore.RefreshBuff))]
        [HarmonyPostfix]
        public static void PostRefreshBuff(TravelStore __instance)
        {
            foreach (var buff in __instance.gameObject.GetComponentsInChildren<TravelBuff>())
            {
                if (buff != null && buff.theBuffType == (int)BuffType.AdvancedBuff &&
                    BuffRegister.CustomAdvancedBuffs.ContainsKey(buff.theBuffNumber))
                {
                    buff.cost = BuffRegister.CustomAdvancedBuffs[buff.theBuffNumber].Item4;
                    buff.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text =
                        $"￥{BuffRegister.CustomAdvancedBuffs[buff.theBuffNumber].Item4}";
                }
            }
        }
    }

    [HarmonyPatch(typeof(TravelBuff))]
    public static class TravelBuffPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("ChangeSprite")]
        public static void PreChangeSprite(TravelBuff __instance)
        {
            if (__instance.theBuffType == (int)BuffType.AdvancedBuff && BuffRegister.CustomAdvancedBuffs.ContainsKey(__instance.theBuffNumber))
                __instance.thePlantType = BuffRegister.CustomAdvancedBuffs[__instance.theBuffNumber].Item1;
        }
    }

    [HarmonyPatch(typeof(NoticeMenu), nameof(NoticeMenu.Start))]
    public static class NoticeMenuPatch
    {
        [HarmonyPostfix]
        [HarmonyPriority(-1900)]
        public static void Postfix()
        {
            BuffCore.InitPlant();
        }
    }
    #endregion

    #region 🤺仙
    [HarmonyPatch(typeof(AbyssSwordStar))]
    public static class AbyssSwordStarPatch
    {
        [HarmonyPatch(nameof(AbyssSwordStar.SetStatus))]
        [HarmonyPrefix]
        public static void PreAttackZombie(AbyssSwordStar __instance, PlantStatus plantStatus)
        {
            if (Lawnf.TravelAdvanced(BuffCore.GetAdvBuffsByPlantType(__instance.thePlantType)[1]))
            {
                if (plantStatus == PlantStatus.AbyssSwordStar_attacking)
                    __instance.attackDamage *= 3;
                else if (plantStatus == PlantStatus.Defalut)
                    __instance.attackDamage /= 3;
            }
        }
    }

    [HarmonyPatch(typeof(Shooter))]
    public static class ShooterPatch
    {
        [HarmonyPatch(nameof(Shooter.Update))]
        [HarmonyPrefix]
        public static void PreUpdate(Shooter __instance)
        {
            if (__instance.thePlantType == PlantType.AbyssSwordStar)
            {
                int num = 0;
                if (BuffCore.GetPlantAdvBuffUnlock(__instance.thePlantType, 0))
                {
                    __instance.thePlantAttackCountDown -= Time.deltaTime / 3;
                    num++;
                }
                if (BuffCore.GetPlantAdvBuffUnlock(__instance.thePlantType, 0))
                {
                    __instance.thePlantAttackCountDown -= Time.deltaTime / 3;
                    num++;
                }
                __instance.anim.speed = 1 + num * 1.75f;
            }
        }
    }
    #endregion
}
