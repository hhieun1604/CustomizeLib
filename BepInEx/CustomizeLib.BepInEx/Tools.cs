using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx.Unity.IL2CPP.Utils;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
#pragma warning disable
namespace CustomizeLib.BepInEx
{
    public static class Tools
    {
        public static Assembly GetAssembly() => Assembly.GetCallingAssembly();
        public static Assembly Assembly {
            get {
                return Assembly.GetCallingAssembly();
            }
        }

        public static void InitMod(bool skipRegister = false) => InitMod(Assembly.GetCallingAssembly(), skipRegister);

        public static void InitMod(Assembly assembly, bool skipRegister = false)
        {
            Console.OutputEncoding = Encoding.UTF8;
            if (!skipRegister)
            {
                var types = GetAllMonoBehaviourTypes(assembly);
                foreach (var type in types)
                    if (!ClassInjector.IsTypeRegisteredInIl2Cpp(type))
                        ClassInjector.RegisterTypeInIl2Cpp(type);
            }
            Harmony.CreateAndPatchAll(assembly);
        }

        public static Type[] GetAllMonoBehaviourTypes(Assembly assembly)
        {
            try
            {
                // 获取所有类型
                Type[] allTypes = assembly.GetTypes();

                // 筛选继承自MonoBehaviour的类型
                return allTypes
                    .Where(type => typeof(MonoBehaviour).IsAssignableFrom(type) &&
                                  !type.IsAbstract &&
                                  !type.IsInterface)
                    .ToArray();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // 处理类型加载异常
                return ex.Types
                    .Where(type => type != null &&
                                  typeof(MonoBehaviour).IsAssignableFrom(type) &&
                                  !type.IsAbstract &&
                                  !type.IsInterface)
                    .ToArray();
            }
        }
    }
    public struct ID
    {
        public int id = 0;
        public ID(int id) { this.id = id; }
        public ID(PlantType id) { this.id = (int)id; }
        public ID(ZombieType id) { this.id = (int)id; }
        public ID(ParticleType id) { this.id = (int)id; }
        public ID(BulletType id) { this.id = (int)id; }
        public ID(CherryBombType id) { this.id = (int)id; }
        public static implicit operator int(ID id) => id.id;
        public static implicit operator PlantType(ID id) => (PlantType)id.id;
        public static implicit operator ZombieType(ID id) => (ZombieType)id.id;
        public static implicit operator ParticleType(ID id) => (ParticleType)id.id;
        public static implicit operator BulletType(ID id) => (BulletType)id.id;
        public static implicit operator CherryBombType(ID id) => (CherryBombType)id.id;
        public static implicit operator ID(int i) => new ID(i);
        public static implicit operator ID(PlantType id) => new ID(id);
        public static implicit operator ID(ZombieType id) => new ID(id);
        public static implicit operator ID(ParticleType id) => new ID(id);
        public static implicit operator ID(BulletType id) => new ID(id);
        public static implicit operator ID(CherryBombType id) => new ID(id);

        public override string ToString()
        {
            return id.ToString();
        }
    }

    public struct BuffID
    {
        public int id = 0;
        public BuffID(int id) { this.id = id; }
        public BuffID(AdvBuff id) { this.id = (int)id; }
        public BuffID(UltiBuff id) { this.id = (int)id; }
        public BuffID(TravelDebuff id) { this.id = (int)id; }
        public BuffID(TravelUnlocks id) { this.id = (int)id; }

        public static implicit operator AdvBuff(BuffID id) => (AdvBuff)id.id;
        public static implicit operator UltiBuff(BuffID id) => (UltiBuff)id.id;
        public static implicit operator TravelDebuff(BuffID id) => (TravelDebuff)id.id;
        public static implicit operator TravelUnlocks(BuffID id) => (TravelUnlocks)id.id;
        public static implicit operator BuffID(AdvBuff i) => new BuffID(i);
        public static implicit operator BuffID(UltiBuff id) => new BuffID(id);
        public static implicit operator BuffID(TravelDebuff id) => new BuffID(id);
        public static implicit operator BuffID(TravelUnlocks id) => new BuffID(id);
        public static implicit operator BuffID(int id) => new BuffID(id);
    }

    public static class Extension
    {
        public static T? GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject != null && gameObject.TryGetComponent<T>(out var component) && component != null)
                return component;
            else if (gameObject != null)
                return gameObject.AddComponent<T>();
            return null;
        }

        public static T? GetOrAddComponent<T>(this Transform gameObject) where T : Component
        {
            if (gameObject != null && gameObject.TryGetComponent<T>(out var component) && component != null)
                return component;
            else if (gameObject != null)
                return gameObject.AddComponent<T>();
            return null;
        }

        public static T? GetOrAddComponent<T>(this Component gameObject) where T : Component
        {
            if (gameObject != null && gameObject.TryGetComponent<T>(out var component) && component != null)
                return component;
            else if (gameObject != null)
                return gameObject.AddComponent<T>();
            return null;
        }

        public static Coroutine StartCoroutine(this MonoBehaviour self, IEnumerator routine)
        {
            return global::BepInEx.Unity.IL2CPP.Utils.MonoBehaviourExtensions.StartCoroutine(self, routine);
        }
    }

    public class CorePlugin : BasePlugin
    {
        public static List<Action> OnGameInitAction = new();

        public ManualLogSource Logger;

        public override void Load()
        {
            Logger = base.Log;
            Tools.InitMod(GetType().Assembly);
            OnStart();
            OnGameInitAction.Add(OnGameInit);
        }

        public virtual void OnStart()
        {

        }

        public virtual void OnGameInit()
        {

        }
    }

    public static class CoreTools
    {
        public static AdvBuff GetAdvBuffByString(string name)
        {
            var id = -1;
            #region 映射
            switch (name)
            {
                case "撒豆成兵":
                    id = 0;
                    break;
                case "精兵强将":
                    id = 1;
                    break;
                case "枕戈待旦":
                    id = 2;
                    break;
                case "核能威慑":
                    id = 3;
                    break;
                case "妙手回春":
                    id = 4;
                    break;
                case "无关痛痒":
                    id = 5;
                    break;
                case "尸愁之路":
                    id = 6;
                    break;
                case "百炼成钢":
                    id = 7;
                    break;
                case "百步穿杨":
                    id = 8;
                    break;
                case "怒火攻心":
                    id = 9;
                    break;
                case "势如破竹":
                    id = 10;
                    break;
                case "冻彻心扉":
                    id = 11;
                    break;
                case "多多益善":
                    id = 12;
                    break;
                case "等价交换":
                    id = 13;
                    break;
                case "人工智能":
                    id = 14;
                    break;
                case "弹射起步":
                    id = 15;
                    break;
                case "特制弹药":
                    id = 16;
                    break;
                case "一针见血":
                    id = 17;
                    break;
                case "可控核聚变":
                    id = 18;
                    break;
                case "人多势众":
                    id = 19;
                    break;
                case "大富翁":
                    id = 20;
                    break;
                case "好运连连":
                    id = 21;
                    break;
                case "量子护盾":
                    id = 22;
                    break;
                case "高能射线":
                    id = 23;
                    break;
                case "枪枪爆头":
                    id = 24;
                    break;
                case "开炮":
                    id = 25;
                    break;
                case "电磁涡轮":
                    id = 26;
                    break;
                case "万磁王":
                    id = 27;
                    break;
                case "罪恶之力":
                    id = 28;
                    break;
                case "深度定制":
                    id = 29;
                    break;
                case "核能射线":
                    id = 30;
                    break;
                case "链式反应":
                    id = 31;
                    break;
                case "尸愁之路II":
                    id = 32;
                    break;
                case "滑滑梯":
                    id = 33;
                    break;
                case "磁力护盾":
                    id = 34;
                    break;
                case "磁能科技":
                    id = 35;
                    break;
                case "至极手速":
                    id = 1000;
                    break;
                case "全息制冷":
                    id = 1001;
                    break;
                case "熠熠生辉":
                    id = 1002;
                    break;
                case "极速战备":
                    id = 1003;
                    break;
                case "复制中心":
                    id = 1004;
                    break;
                case "运斤如风":
                    id = 1005;
                    break;
                case "全副武装":
                    id = 1006;
                    break;
                case "致命一击":
                    id = 1007;
                    break;
                case "强力打击":
                    id = 1008;
                    break;
                case "精准打击":
                    id = 1009;
                    break;
                case "合理投资":
                    id = 1010;
                    break;
                case "极致之冰":
                    id = 1011;
                    break;
                case "聚光盆":
                    id = 1012;
                    break;
                case "真樱":
                    id = 1013;
                    break;
                case "拆分":
                    id = 1014;
                    break;
                case "连连看":
                    id = 1015;
                    break;
                case "肉身成圣":
                    id = 1016;
                    break;
                case "斗转星移":
                    id = 1017;
                    break;
                case "灯火通明":
                    id = 1018;
                    break;
                case "真毁":
                    id = 2000;
                    break;
                case "排山倒海":
                    id = 2001;
                    break;
                case "升级":
                    id = 2002;
                    break;
                case "绝对力量":
                    id = 2003;
                    break;
                case "爽快射击":
                    id = 2004;
                    break;
                case "生命偷取":
                    id = 2005;
                    break;
                case "过载":
                    id = 2006;
                    break;
                case "腐化":
                    id = 2007;
                    break;
                case "净化":
                    id = 2008;
                    break;
                case "我是传奇":
                    id = 3000;
                    break;
                case "星神合一":
                    id = 3001;
                    break;
                case "火力全开":
                    id = 3002;
                    break;
                case "星月护符":
                    id = 3003;
                    break;
                case "永寂新星":
                    id = 3004;
                    break;
                case "力量会给予希望":
                    id = 3005;
                    break;
                case "无限火力":
                    id = 3006;
                    break;
                case "迷你巨人":
                    id = 4000;
                    break;
                case "迷你将军":
                    id = 4001;
                    break;
                case "迷你雪皇":
                    id = 4002;
                    break;
                case "迷你丑皇":
                    id = 4003;
                    break;
                case "Curse_巨人杀手":
                    id = 5000;
                    break;
                case "Curse_诅咒之力":
                    id = 5001;
                    break;
                case "Curse_诸神黄昏":
                    id = 5002;
                    break;
                case "Curse_荆狂诅咒":
                    id = 5003;
                    break;
                case "Curse_贪婪诅咒":
                    id = 5004;
                    break;
                case "Rogue_后备能源":
                    id = 6000;
                    break;
                case "Rogue_究极樱桃战神专精I":
                    id = 6001;
                    break;
                case "Rogue_究极樱桃战神专精II":
                    id = 6002;
                    break;
                case "Rogue_究极樱桃射手专精I":
                    id = 6003;
                    break;
                case "Rogue_究极樱桃射手专精II":
                    id = 6004;
                    break;
                case "Rogue_究极大喷菇专精I":
                    id = 6005;
                    break;
                case "Rogue_究极大喷菇专精II":
                    id = 6006;
                    break;
                case "Rogue_究极窝炬专精I":
                    id = 6007;
                    break;
                case "Rogue_究极窝炬专精II":
                    id = 6008;
                    break;
                case "Rogue_推车保护":
                    id = 6009;
                    break;
            }
            #endregion
            return (AdvBuff)id;
        }


        public static UltiBuff GetUltiBuffByString(string name)
        {
            var id = -1;
            #region 映射
            switch (name)
            {
                case "嗜血如命":
                    id = 0;
                    break;
                case "极速吞噬":
                    id = 1;
                    break;
                case "力大砖飞":
                    id = 2;
                    break;
                case "快速填装":
                    id = 3;
                    break;
                case "凛风刺骨":
                    id = 4;
                    break;
                case "三尺之寒":
                    id = 5;
                    break;
                case "事半功倍":
                    id = 6;
                    break;
                case "窝红温了":
                    id = 7;
                    break;
                case "流星雨":
                    id = 8;
                    break;
                case "众星之力":
                    id = 9;
                    break;
                case "万籁俱寂":
                    id = 10;
                    break;
                case "以爆制爆":
                    id = 11;
                    break;
                case "见者有份":
                    id = 12;
                    break;
                case "蒜毒骤发":
                    id = 13;
                    break;
                case "中心爆破":
                    id = 14;
                    break;
                case "兵贵神速":
                    id = 15;
                    break;
                case "世纪之盾":
                    id = 16;
                    break;
                case "两肋插刀":
                    id = 17;
                    break;
                case "普度众生":
                    id = 18;
                    break;
                case "永动机":
                    id = 19;
                    break;
                case "阻冲之":
                    id = 20;
                    break;
                case "狂战士":
                    id = 21;
                    break;
                case "金光闪闪":
                    id = 22;
                    break;
                case "人造太阳":
                    id = 23;
                    break;
                case "三位一体":
                    id = 24;
                    break;
                case "镭射激光":
                    id = 25;
                    break;
                case "气定神闲":
                    id = 26;
                    break;
                case "僵尸试图在火海中游泳":
                    id = 27;
                    break;
                case "厚积薄发":
                    id = 28;
                    break;
                case "打折券":
                    id = 29;
                    break;
                case "无尽贪婪":
                    id = 30;
                    break;
                case "万劫不复":
                    id = 31;
                    break;
                case "延时飞刃":
                    id = 32;
                    break;
                case "生机永驻":
                    id = 33;
                    break;
                case "深渊巨口":
                    id = 34;
                    break;
                case "光芒四射":
                    id = 35;
                    break;
                case "爆米花":
                    id = 36;
                    break;
                case "超载":
                    id = 37;
                    break;
                case "连锁反应":
                    id = 38;
                    break;
                case "大容量":
                    id = 39;
                    break;
                case "集中轰炸":
                    id = 40;
                    break;
                case "聚精会神":
                    id = 41;
                    break;
                case "炙热射线":
                    id = 42;
                    break;
                case "三昧真火":
                    id = 43;
                    break;
                case "深海恐惧":
                    id = 44;
                    break;
                case "群体出动":
                    id = 45;
                    break;
                case "斩将祭旗":
                    id = 46;
                    break;
                case "黑曜护体":
                    id = 47;
                    break;
            }
            #endregion
            return (UltiBuff)id;
        }


        public static TravelDebuff GetTravelDebuffByString(string name)
        {
            var id = -1;
            #region 映射
            switch (name)
            {
                case "二爷1":
                    id = 0;
                    break;
                case "二爷2":
                    id = 1;
                    break;
                case "黑橄榄1":
                    id = 2;
                    break;
                case "黑橄榄2":
                    id = 3;
                    break;
                case "机鱼1":
                    id = 4;
                    break;
                case "机鱼2":
                    id = 5;
                    break;
                case "基洛夫1":
                    id = 6;
                    break;
                case "基洛夫2":
                    id = 7;
                    break;
                case "丑跳1":
                    id = 8;
                    break;
                case "丑跳2":
                    id = 9;
                    break;
                case "三叉戟1":
                    id = 10;
                    break;
                case "三叉戟2":
                    id = 11;
                    break;
                case "白舞王1":
                    id = 12;
                    break;
                case "白舞王2":
                    id = 13;
                    break;
                case "尸王1":
                    id = 14;
                    break;
                case "尸王2":
                    id = 15;
                    break;
                case "冲车1":
                    id = 16;
                    break;
                case "植物概率死亡":
                    id = 17;
                    break;
                case "阳光归零":
                    id = 18;
                    break;
                case "血量翻倍":
                    id = 19;
                    break;
                case "更多僵尸":
                    id = 20;
                    break;
                case "黑车1":
                    id = 21;
                    break;
                case "黑车2":
                    id = 22;
                    break;
                case "博士":
                    id = 23;
                    break;
                case "究极马超":
                    id = 24;
                    break;
                case "究极鱼丸":
                    id = 25;
                    break;
                case "究极将军":
                    id = 26;
                    break;
                case "究极裂空":
                    id = 27;
                    break;
                case "究极白车":
                    id = 28;
                    break;
                case "究极读报":
                    id = 29;
                    break;
                case "蹦极":
                    id = 30;
                    break;
                case "究极丑皇":
                    id = 31;
                    break;
                case "复活":
                    id = 32;
                    break;
                case "植物数量限制":
                    id = 33;
                    break;
                case "究极阿尔法":
                    id = 34;
                    break;
                case "阿尔法1":
                    id = 35;
                    break;
                case "冲车2":
                    id = 36;
                    break;
                case "究极大帅":
                    id = 37;
                    break;
                case "旅行飞碟1":
                    id = 38;
                    break;
                case "旅行飞碟2":
                    id = 39;
                    break;
                case "特种巨人1":
                    id = 40;
                    break;
                case "特种巨人2":
                    id = 41;
                    break;
                case "堡垒巨人1":
                    id = 42;
                    break;
                case "特种黄金巨人":
                    id = 43;
                    break;
                case "毁灭机枪二爷":
                    id = 44;
                    break;
                case "鱼丸1":
                    id = 45;
                    break;
                case "鱼丸2":
                    id = 46;
                    break;
                case "重症难题":
                    id = 47;
                    break;
                case "永久创伤":
                    id = 48;
                    break;
            }
            #endregion
            return (TravelDebuff)id;
        }

        public static TravelUnlocks GetTravelUnlocksByString(string name)
        {
            var id = -1;
            #region 映射
            switch (name)
            {
                case "UltimateChomper":
                    id = 0;
                    break;
                case "UltimateGatling":
                    id = 1;
                    break;
                case "UltimateFume":
                    id = 2;
                    break;
                case "UltimateTorch":
                    id = 3;
                    break;
                case "UltimateStar":
                    id = 4;
                    break;
                case "UltimateGloom":
                    id = 5;
                    break;
                case "UltimateMelon":
                    id = 6;
                    break;
                case "UltimateCannon":
                    id = 7;
                    break;
                case "UltimateTallNut":
                    id = 8;
                    break;
                case "UltimateHypno":
                    id = 9;
                    break;
                case "UltimateBigGatling":
                    id = 10;
                    break;
                case "UltimateCabbage":
                    id = 11;
                    break;
                case "UltimatePumpkin":
                    id = 12;
                    break;
                case "UltimateSpring":
                    id = 13;
                    break;
                case "UltimateKelp":
                    id = 14;
                    break;
                case "UltimateCorn":
                    id = 15;
                    break;
                case "UltimateSpruce":
                    id = 16;
                    break;
                case "UltimateBigChomper":
                    id = 17;
                    break;
                case "UltimateExplodeCannon":
                    id = 18;
                    break;
                case "UltimateSunflower":
                    id = 19;
                    break;
                case "UltimateWinterMelon":
                    id = 20;
                    break;
                case "UltimateCattail":
                    id = 21;
                    break;
                case "UltimateSeaShroom":
                    id = 22;
                    break;
                case "UltimateJalapeno":
                    id = 23;
                    break;
            }
            #endregion
            return (TravelUnlocks)id;
        }
    }
}
