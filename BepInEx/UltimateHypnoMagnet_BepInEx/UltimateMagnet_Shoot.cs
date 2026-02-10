using System;
using HarmonyLib;

namespace UltimateHypnoMagnet_BepInEx
{
	// Token: 0x02000004 RID: 4
	[HarmonyPatch(typeof(UltimateMagnet))]
	public class UltimateMagnet_Shoot
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002B7C File Offset: 0x00000D7C
		[HarmonyPatch("Shoot")]
		[HarmonyPrefix]
		public static bool Prefix(UltimateMagnet __instance, ref UltimateMagnet.AttrackedBucket bucket)
		{
			bool flag = __instance != null && __instance.thePlantType == UltimateHypnoMagnet.PlantID;
			bool result;
			if (flag)
			{
				UltimateHypnoMagnet component = __instance.GetComponent<UltimateHypnoMagnet>();
				component.SpawnZombie(bucket.bucket.theBucketType, bucket);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
