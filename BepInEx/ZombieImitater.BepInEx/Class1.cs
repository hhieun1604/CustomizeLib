using BepInEx;
using BepInEx.Unity.IL2CPP;
using CustomizeLib.BepInEx;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using static Il2CppSystem.Globalization.CultureInfo;
using Random = UnityEngine.Random;

namespace ZombieImitater.BepInEx
{
    [BepInPlugin("salmon.zombieinitater", "ZombieImitater", "1.0")]
    public class Core : BasePlugin//960
    {
        public override void Load()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            ClassInjector.RegisterTypeInIl2Cpp<ZombieImitater>();
            ClassInjector.RegisterTypeInIl2Cpp<ClearCold>();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var ab = CustomCore.GetAssetBundle(Assembly.GetExecutingAssembly(), "zombieimitater");
            CustomCore.RegisterCustomPlant<Imitater, ZombieImitater>(ZombieImitater.PlantID, ab.GetAsset<GameObject>("ZombieimitaterPrefab"),
                ab.GetAsset<GameObject>("ZombieimitaterPreview"), [], 0f, 0f, 0, 300, 30f, -500);
            CustomCore.RegisterCustomPlant<Imitater, ZombieImitater>(ZombieImitater.PlantIDRed, ab.GetAsset<GameObject>("ZombieimitaterPrefabRed"),
                ab.GetAsset<GameObject>("ZombieimitaterPreviewRed"), [], 0f, 0f, 0, 300, 30f, -500);
            CustomCore.AddPlantAlmanacStrings(ZombieImitater.PlantID, $"僵尸模仿者",
                "面目僵冷，祸源已凝。\n\n" +
                "<color=#3D1400>贴图作者：@林秋-AutumnLin</color>\n" +
                "<color=#3D1400>特点：</color><color=red>①无法被铲除/被手套移动\n" +
                "②不会被僵尸索敌\n" +
                "③出场1.5秒后，生成一位随机僵尸，或特殊产出\n" +
                "④若僵尸模仿者卡片消失后未种植，随机在五列右侧出现一株僵尸模仿者</color>\n" +
                "<color=#3D1400>概率明细：</color>\n" +
                "<color=red>各类普通僵尸（60%）；究极僵尸（20%）；领袖及Boss僵尸（10%）；特殊事件（5%）；钻石模仿者（5%）。生成的僵尸随机具有0.8/1/1.2倍韧性</color>\n" +
                "<color=#3D1400>事件明细：</color>\n" +
                "<color=red>①获得一个词条：僵尸词条（80%）；普通词条（10%）；强究词条（10%）\n" +
                "②生成一个钻石模仿者</color>\n" +
                "<color=#3D1400>特殊强化：</color><color=red>①<Boss>僵王博士：血量x100\n" +
                "②<Boss>黄金僵王博士：血量x100\n" +
                "③<Boss>黑橄榄大帅：血量x15</color>\n\n" +
                "<color=#3D1400>宝开🐟</color>\n\n" +
                "<color=#955300>价格：</color><color=red>-500</color>\n" +
                "<color=#955300>冷却：</color><color=red>30秒</color>"); 
            CustomCore.AddPlantAlmanacStrings(ZombieImitater.PlantIDRed, $"红眼僵尸模仿者",
                "血染双目，灾殃将至。\n\n" +
                "<color=#3D1400>贴图作者：@林秋-AutumnLin</color>\n" +
                "<color=#3D1400>使用条件：</color><color=red>种植僵尸模仿者时有50%变异</color>\n" +
                "<color=#3D1400>特点：</color><color=red>拥有僵尸模仿者的全部特点</color>\n\n" +
                "<color=#3D1400>宝开fish</color>\n\n" +
                "<color=#955300>价格：</color><color=red>-500</color>\n" +
                "<color=#955300>冷却：</color><color=red>30秒</color>");
            CustomCore.RegisterCustomCardToColorfulCards((PlantType)ZombieImitater.PlantID);
        }
    }

    public class ZombieImitater : MonoBehaviour
    {
        public static ID PlantID = 1960;
        public static ID PlantIDRed = 1961;

        public Imitater plant => gameObject.GetComponent<Imitater>();

        public void AnimSpawn()
        {
            ParticleManager.Instance.SetParticle(ParticleType.RandomCloud, plant.axis.transform.position, plant.thePlantRow, lim: true);
            int row = plant.thePlantRow;
            int column = plant.thePlantColumn;
            var axis = plant.axis.transform.position;
            var x = axis.x;
            plant.Die();
            var v = UnityEngine.Random.Range(1, 101);
            var list = GetAllowZombies();
            if (v <= 60) // 普通
            {
                list = list.Where(t => !TypeMgr.UltimateZombie(t) && !TypeMgr.IsBossZombie(t)).ToList();
                SetZombie(list[Random.Range(0, list.Count)], x, row);
            }
            else if (v <= 80) // 究极
            {
                list = list.Where(t => TypeMgr.UltimateZombie(t) && !TypeMgr.IsBossZombie(t)).ToList();
                SetZombie(list[Random.Range(0, list.Count)], x, row);
            }
            else if (v <= 90) // 领袖
            {
                list = list.Where(t => TypeMgr.IsBossZombie(t)).ToList();
                SetZombie(list[Random.Range(0, list.Count)], x, row);
            }
            else if (v <= 95) // event
            {
                TriggerEvent(row, column);
            }
            else // 钻闪闪
            {
                CreatePlant.Instance.SetPlant(column, row, PlantType.DiamondImitater, isFreeSet: true);
            }
        }

        public void TriggerEvent(int row, int column)
        {
            if (Random.Range(0, 2) == 0) // 词条
            {
                int v = Random.Range(1, 101);
                var mgr = GameAPP.gameAPP.GetOrAddComponent<TravelMgr>();
                var data = mgr.data;
                if (v <= 80) // 僵尸
                {
                    var list = new List<TravelDebuff>();
                    foreach (var (id, _) in TravelDictionary.debuffData)
                        if (!data.travelDebuffs.Contains(id))
                            list.Add(id);
                    var debuff = list[UnityEngine.Random.Range(0, list.Count)];
                    data.travelDebuffs.Add(debuff);
                    InGameText.Instance.ShowText($"抽到僵尸词条：{TravelDictionary.debuffData[debuff].Item1}", 5f);
                }
                else if (v <= 90) // 普通词条
                {
                    var list = new List<UltiBuff>();
                    foreach (var (id, _) in TravelDictionary.ultimateBuffsText)
                        if (!data.ultiBuffs.Contains(id) && !data.ultiBuffs.Contains(id))
                            list.Add(id);
                    var ultiBuff = list[UnityEngine.Random.Range(0, list.Count)];
                    data.ultiBuffs.Add(ultiBuff);
                    InGameText.Instance.ShowText($"抽到强究词条：{TravelDictionary.ultimateBuffsText[ultiBuff]}", 5f);
                }
                else // 强究词条
                {
                    var list = new List<AdvBuff>();
                    foreach (var (id, _) in TravelDictionary.advancedBuffsText)
                        if (!data.advBuffs.Contains(id) && !data.advBuffs_lv2.Contains(id))
                            list.Add(id);
                    var advBuff = list[UnityEngine.Random.Range(0, list.Count)];
                    data.advBuffs.Add(advBuff);
                    InGameText.Instance.ShowText($"抽到普通词条：{TravelDictionary.advancedBuffsText[advBuff]}", 5f);
                }
            }
            else // 生成钻闪闪
            {
                CreatePlant.Instance.SetPlant(column, row, PlantType.DiamondImitater, isFreeSet: true);
            }
        }

        public void SetZombie(ZombieType type, float x, int row)
        {
            int nR = row;
            if (type == ZombieType.ZombieBoss || type == ZombieType.ZombieBoss2)
                nR = 0;
            if (type == ZombieType.ZombieBoss)
                GameAPP.Instance.PlayMusic(MusicType.Boss);
            if (type == ZombieType.ZombieBoss2)
                GameAPP.Instance.PlayMusic(MusicType.Boss2);
            var tmp = Board.Instance.isEveStarted;
            Board.Instance.isEveStarted = true;
            try
            {
                var zombie = CreateZombie.Instance.SetZombie(nR, type, x).GetComponent<Zombie>();
                Board.Instance.isEveStarted = tmp;
                if (zombie == null) return;
                float timer = 1f;
                switch (UnityEngine.Random.Range(0, 3))
                {
                    case 0:
                        timer = 0.8f;
                        break;
                    case 1:
                        timer = 1.2f;
                        break;
                }
                zombie.theHealth = (int)(zombie.theHealth * timer);
                zombie.theMaxHealth = (int)(zombie.theMaxHealth * timer);
                zombie.theFirstArmorHealth = (int)(zombie.theFirstArmorHealth * timer);
                zombie.theFirstArmorMaxHealth = (int)(zombie.theFirstArmorMaxHealth * timer);
                zombie.theSecondArmorHealth = (int)(zombie.theSecondArmorHealth * timer);
                zombie.theSecondArmorMaxHealth = (int)(zombie.theSecondArmorMaxHealth * timer);
                if (type == ZombieType.ZombieBoss || type == ZombieType.ZombieBoss2)
                {
                    zombie.theHealth *= 100;
                    zombie.theMaxHealth *= 100;
                }
                if (type == ZombieType.HorseBoss)
                {
                    zombie.theHealth *= 15;
                    zombie.theMaxHealth *= 15;
                }
            }
            catch (Exception)
            { }
        }

        public List<ZombieType> GetAllowZombies()
        {
            return GameAPP.resourcesManager.allZombieTypes.ToSystemList().Where(t => t != ZombieType.TrainingDummy && t != ZombieType.ProjectileZombie).ToList();
        }
    }

    public class ClearCold : MonoBehaviour
    {
        public Zombie zombie;

        public void Update()
        {
            if (zombie != null && !zombie.IsDestroyed())
            {
                zombie.freezeLevel = 0;
                if (zombie.coldTimer > 0f)
                {
                    zombie.UnCold();
                    zombie.coldTimer = 0f;
                }
                if (zombie.freezeTimer > 0f)
                {
                    zombie.freezeTimer = 0f;
                    zombie.Unfreezing();
                }
            }
        }
    }

    [HarmonyPatch(typeof(CreatePlant), nameof(CreatePlant.SetPlant))]
    public static class CreatePlantPatch
    {
        [HarmonyPrefix]
        public static void Prefix(ref PlantType theSeedType)
        {
            if (theSeedType == ZombieImitater.PlantID && UnityEngine.Random.Range(0, 2) == 0)
                theSeedType = ZombieImitater.PlantIDRed;
        }
    }

    [HarmonyPatch(typeof(DroppedCard), nameof(DroppedCard.Update))]
    public static class DroppedCardPatch
    {
        [HarmonyPrefix]
        public static void Prefix(DroppedCard __instance)
        {
            if (__instance.existTime + Time.deltaTime >= 15f && GameAPP.theGameStatus == GameStatus.InGame && Time.timeScale > 0f && __instance.thePlantType == ZombieImitater.PlantID)
            {
                for (int i = 4; i < Board.Instance.columnNum; i++) // 列
                {
                    for (int j = 0; j < Board.Instance.rowNum; j++) // 行
                    {
                        if (Lawnf.Get1x1Plants(i, j).Count <= 0)
                        {
                            ParticleManager.Instance.SetParticle(ParticleType.RandomCloud, new Vector2(
                                Mouse.Instance.GetBoxXFromColumn(i),
                                Mouse.Instance.GetBoxYFromRow(j)));
                            CreatePlant.Instance.SetPlant(i, j, ZombieImitater.PlantID, isFreeSet: true);
                            return;
                        }
                    }
                }
                int column = Random.Range(4, Board.Instance.columnNum);
                int row = Random.Range(0, Board.Instance.rowNum);
                foreach (var plant in Lawnf.Get1x1Plants(column, row))
                {
                    if (plant == null) continue;
                    if (TypeMgr.IsLily(plant.thePlantType) || TypeMgr.IsPot(plant.thePlantType)) continue;
                    plant.Die();
                }
                ParticleManager.Instance.SetParticle(ParticleType.RandomCloud, new Vector2(
                    Mouse.Instance.GetBoxXFromColumn(column),
                    Mouse.Instance.GetBoxYFromRow(row)));
                CreatePlant.Instance.SetPlant(column, row, ZombieImitater.PlantID, isFreeSet: true);
            }
        }
    }

    [HarmonyPatch(typeof(CardUI), nameof(CardUI.Start))]
    public class CardUI_Start_Patch
    {
        public static int loopCount = 0;
        [HarmonyPrefix]
        public static void Postfix(CardUI __instance)
        {
            if (__instance.thePlantType == (PlantType)ZombieImitater.PlantID && loopCount < 14)
            {
                GameObject go = GameObject.Instantiate(__instance.gameObject, __instance.transform.parent);
                go.transform.position = __instance.transform.position;
                __instance.CD = __instance.fullCD;
                go.GetComponent<CardUI>().CD = go.GetComponent<CardUI>().fullCD;
                loopCount++;
            }
        }
    }

    [HarmonyPatch(typeof(Board), nameof(Board.Start))]
    public class Board_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix() => CardUI_Start_Patch.loopCount = 0;
    }

    [HarmonyPatch(typeof(ZombieBoss), nameof(ZombieBoss.Start))]
    public static class ZombieBossStartPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ZombieBoss __instance)
        {
            {
                var position = __instance.axis.transform.position;
                position.y -= Lawnf.GetAllZombies().ToSystemList().Where(z => z.theZombieType == ZombieType.ZombieBoss || z.theZombieType == ZombieType.ZombieBoss2)
                    .ToList().Count * 0.4f;
                __instance.healthText.transform.position = position;
            }
            {
                __instance.healthTextShadow.transform.position = __instance.healthText.transform.position;
            }
        }
    }
}
