using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace UltimateHypnoMagnet_BepInEx
{
	// Token: 0x02000003 RID: 3
	public class UltimateHypnoMagnet : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000021FD File Offset: 0x000003FD
		public UltimateHypnoMagnet() : base(ClassInjector.DerivedConstructorPointer<UltimateHypnoMagnet>())
		{
			ClassInjector.DerivedConstructorBody(this);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002212 File Offset: 0x00000412
		public UltimateHypnoMagnet(IntPtr i) : base(i)
		{
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002220 File Offset: 0x00000420
		public void Start()
		{
			bool flag = UltimateHypnoMagnet.types == null;
			if (flag)
			{
				UltimateHypnoMagnet.types = new List<ZombieType>();
				foreach (ZombieType zombieType in GameAPP.resourcesManager.allZombieTypes)
				{
					bool flag2 = TypeMgr.UltimateZombie(zombieType);
					if (flag2)
					{
						UltimateHypnoMagnet.types.Add(zombieType);
					}
				}
				foreach (ZombieType item in TypeMgr.UltiZombie_level_a)
				{
					bool flag3 = !UltimateHypnoMagnet.types.Contains(item);
					if (flag3)
					{
						UltimateHypnoMagnet.types.Add(item);
					}
				}
				foreach (ZombieType item2 in TypeMgr.UltiZombie_level_b)
				{
					bool flag4 = !UltimateHypnoMagnet.types.Contains(item2);
					if (flag4)
					{
						UltimateHypnoMagnet.types.Add(item2);
					}
				}
				foreach (ZombieType item3 in TypeMgr.UltiZombie_level_c)
				{
					bool flag5 = !UltimateHypnoMagnet.types.Contains(item3);
					if (flag5)
					{
						UltimateHypnoMagnet.types.Add(item3);
					}
				}
				bool flag6 = UltimateHypnoMagnet.types.Contains(320);
				if (flag6)
				{
					UltimateHypnoMagnet.types.Remove(320);
				}
				bool flag7 = UltimateHypnoMagnet.types.Contains(319);
				if (flag7)
				{
					UltimateHypnoMagnet.types.Remove(319);
				}
				bool flag8 = UltimateHypnoMagnet.types.Contains(318);
				if (flag8)
				{
					UltimateHypnoMagnet.types.Remove(318);
				}
				bool flag9 = UltimateHypnoMagnet.types.Contains(28);
				if (flag9)
				{
					UltimateHypnoMagnet.types.Remove(28);
				}
				bool flag10 = UltimateHypnoMagnet.types.Contains(226);
				if (flag10)
				{
					UltimateHypnoMagnet.types.Remove(226);
				}
				bool flag11 = UltimateHypnoMagnet.types.Contains(43);
				if (flag11)
				{
					UltimateHypnoMagnet.types.Remove(43);
				}
				for (int i = 0; i < UltimateHypnoMagnet.types.Count; i++)
				{
					bool flag12 = TypeMgr.IsBossZombie(UltimateHypnoMagnet.types[i]);
					if (flag12)
					{
						UltimateHypnoMagnet.types.RemoveAt(i);
						i--;
					}
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002468 File Offset: 0x00000668
		public void SpawnZombie(BucketType bucket, UltimateMagnet.AttrackedBucket item = null)
		{
			bool flag = !Lawnf.TravelAdvanced(1);
			if (flag)
			{
				Zombie zombie = null;
				switch (bucket)
				{
				case 0:
				{
					int num = Random.Range(0, 3);
					bool flag2 = num == 0;
					if (flag2)
					{
						zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 4, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
					}
					else
					{
						bool flag3 = num == 1;
						if (flag3)
						{
							zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 106, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
						}
						else
						{
							zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 114, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
						}
					}
					break;
				}
				case 1:
				{
					int num2 = Random.Range(0, 3);
					bool flag4 = num2 == 0;
					if (flag4)
					{
						zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 9, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
					}
					else
					{
						bool flag5 = num2 == 1;
						if (flag5)
						{
							zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 109, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
						}
						else
						{
							zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 118, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
						}
					}
					break;
				}
				case 2:
				{
					int num3 = Random.Range(0, 2);
					bool flag6 = num3 == 0;
					if (flag6)
					{
						zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 24, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
					}
					else
					{
						zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 30, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
					}
					break;
				}
				case 3:
				{
					MinerZombie component = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 28, this.plant.axis.transform.position.x, false).GetComponent<MinerZombie>();
					component.theStatus = 13;
					component.Rise();
					zombie = component;
					break;
				}
				case 4:
					zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 40, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
					break;
				case 5:
					zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 210, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
					break;
				case 6:
				{
					PogoZombie component2 = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 32, this.plant.axis.transform.position.x, false).GetComponent<PogoZombie>();
					component2.LoseJumper(0);
					zombie = component2;
					break;
				}
				case 7:
					zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 33, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
					break;
				case 8:
					zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 38, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
					break;
				case 9:
					zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 39, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
					break;
				case 10:
				{
					int num4 = Random.Range(0, 2);
					bool flag7 = num4 == 0;
					if (flag7)
					{
						zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 8, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
					}
					else
					{
						zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 114, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
					}
					break;
				}
				case 11:
					zombie = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, 52, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
					break;
				}
				bool flag8 = zombie != null && Lawnf.TravelAdvanced(53);
				if (flag8)
				{
					zombie.theAttackDamage *= 4;
					zombie.theMaxHealth *= 4;
					zombie.theHealth = zombie.theMaxHealth;
					zombie.theFirstArmorMaxHealth *= 4;
					zombie.theFirstArmorHealth = zombie.theFirstArmorMaxHealth;
					zombie.theSecondArmorMaxHealth *= 4;
					zombie.theSecondArmorHealth = zombie.theSecondArmorMaxHealth;
					zombie.UpdateHealthText();
				}
			}
			else
			{
				ZombieType zombieType = UltimateHypnoMagnet.types[Random.Range(0, UltimateHypnoMagnet.types.Count)];
				Zombie component3 = CreateZombie.Instance.SetZombieWithMindControl(this.plant.thePlantRow, zombieType, this.plant.axis.transform.position.x, false).GetComponent<Zombie>();
				bool flag9 = Lawnf.TravelAdvanced(53);
				if (flag9)
				{
					component3.theAttackDamage *= 4;
					component3.theMaxHealth *= 4;
					component3.theHealth = component3.theMaxHealth;
					component3.theFirstArmorMaxHealth *= 4;
					component3.theFirstArmorHealth = component3.theFirstArmorMaxHealth;
					component3.theSecondArmorMaxHealth *= 4;
					component3.theSecondArmorHealth = component3.theSecondArmorMaxHealth;
					component3.UpdateHealthText();
				}
			}
			bool flag10 = item != null;
			if (flag10)
			{
				item.die = true;
			}
			bool flag11 = ParticleManager.Instance != null;
			if (flag11)
			{
				ParticleManager.Instance.SetParticle(11, this.plant.axis.transform.position, this.plant.thePlantRow);
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002B3B File Offset: 0x00000D3B
		public void Awake()
		{
			this.plant.shoot = base.transform.FindChild("Shoot");
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002B5A File Offset: 0x00000D5A
		public UltimateMagnet plant
		{
			get
			{
				return base.gameObject.GetComponent<UltimateMagnet>();
			}
		}

		// Token: 0x04000001 RID: 1
		public static List<ZombieType> types = null;

		// Token: 0x04000002 RID: 2
		public static int PlantID = 1915;
	}
}
