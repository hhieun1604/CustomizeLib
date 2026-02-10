using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using CustomizeLib.BepInEx;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace UltimateHypnoMagnet_BepInEx
{
	// Token: 0x02000002 RID: 2
	[BepInPlugin("salmon.ultimatehypnomagnet", "UltimateHypnoMagnet", "1.0")]
	public class Core : BasePlugin
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void Load()
		{
			Console.OutputEncoding = Encoding.UTF8;
			Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
			ClassInjector.RegisterTypeInIl2Cpp<UltimateHypnoMagnet>();
			Console.OutputEncoding = Encoding.UTF8;
			AssetBundle assetBundle = CustomCore.GetAssetBundle(Assembly.GetExecutingAssembly(), "ultimatehypnomagnet");
			CustomCore.RegisterCustomPlant<UltimateMagnet, UltimateHypnoMagnet>(UltimateHypnoMagnet.PlantID, assetBundle.GetAsset("UltimateHypnoMagnetPrefab"), assetBundle.GetAsset("UltimateHypnoMagnetPreview"), new List<ValueTuple<int, int>>
			{
				new ValueTuple<int, int>(944, 8),
				new ValueTuple<int, int>(8, 944)
			}, 0.5f, 0f, 300, 300, 0f, 450);
			CustomCore.TypeMgrExtra.IsMagnetPlants.Add(UltimateHypnoMagnet.PlantID);
			CustomCore.AddFusion(944, UltimateHypnoMagnet.PlantID, 2);
			CustomCore.AddFusion(944, 2, UltimateHypnoMagnet.PlantID);
			int plantID = UltimateHypnoMagnet.PlantID;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler.AppendLiteral("魅惑磁力菇王(");
			defaultInterpolatedStringHandler.AppendFormatted<int>(UltimateHypnoMagnet.PlantID);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			CustomCore.AddPlantAlmanacStrings(plantID, defaultInterpolatedStringHandler.ToStringAndClear(), "磁性极强，快速吸引铁器并将其转化为对应的魅惑僵尸。\n\n<color=#3D1400>贴图作者：@林秋-AutumnLin</color>\n<color=#3D1400>转换配方：</color><color=red>樱桃炸弹←→魅惑菇</color>\n<color=#3D1400>特点：</color><color=red>磁力菇王亚种。拥有磁力菇王和魅惑磁力菇的特点，吸取间隔为0.5秒，吸取铁器3秒后转化为对应魅惑僵尸，每0.5秒额外伤害吸取范围内所有的橄榄三叉戟类僵尸或黑橄榄类僵尸。</color>\n<color=#3D1400>词条1：</color><color=red>电磁涡轮：吸取半径翻倍。</color>\n<color=#3D1400>词条2：</color><color=red>万磁王：一次性可吸3个铁器，且召唤的僵尸血量和啃咬伤害+300%。</color>\n<color=#3D1400>词条3：</color><color=red>精兵强将：魅惑磁力菇王将召唤随机究极僵尸。</color>\n\n<color=#3D1400>作为植物界的“大魔术师”，魅惑磁力菇王最精湛的演技就是将铁器变成僵尸，不过被召唤的僵尸都以奇怪的姿势瘫在了舞台上。</color>");
			using (IEnumerator enumerator = Enum.GetValues(typeof(BucketType)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BucketType bucketType = (BucketType)enumerator.Current;
					CustomCore.RegisterCustomUseItemOnPlantEvent(UltimateHypnoMagnet.PlantID, bucketType, delegate(Plant plant)
					{
						bool flag = plant != null && plant.thePlantType == UltimateHypnoMagnet.PlantID;
						if (flag)
						{
							UltimateHypnoMagnet component = plant.GetComponent<UltimateHypnoMagnet>();
							component.SpawnZombie(bucketType, null);
						}
					});
				}
			}
		}
	}
}
