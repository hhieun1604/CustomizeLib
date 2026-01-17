using MelonLoader;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace CustomizeLib.MelonLoader
{
    public class Tools
    {
        public static Assembly GetAssembly() => Assembly.GetCallingAssembly();
        public static Assembly Assembly
        {
            get
            {
                return Assembly.GetCallingAssembly();
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
    }

    public struct BuffID
    {
        public int id = 0;
        public BuffID(int id) { this.id = id; }
        public BuffID(AdvBuff id) { this.id = (int)id; }
        public BuffID(UltiBuffs id) { this.id = (int)id; }
        public BuffID(TravelDebuff id) { this.id = (int)id; }

        public static implicit operator AdvBuff(BuffID id) => (AdvBuff)id.id;
        public static implicit operator UltiBuffs(BuffID id) => (UltiBuffs)id.id;
        public static implicit operator TravelDebuff(BuffID id) => (TravelDebuff)id.id;
        public static implicit operator BuffID(AdvBuff i) => new BuffID(i);
        public static implicit operator BuffID(UltiBuffs id) => new BuffID(id);
        public static implicit operator BuffID(TravelDebuff id) => new BuffID(id);
        public static implicit operator BuffID(int id) => new BuffID(id);
    }

    public static class CoroutineRunner
    {
        public static object Start(IEnumerator routine, MonoBehaviour behaviour = null)
        {
            return MelonCoroutines.Start(routine);
        }

        public static void Stop(object routine, MonoBehaviour behaviour = null)
        {
            MelonCoroutines.Stop(routine);
        }
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
            return (Coroutine)MelonCoroutines.Start(routine);
        }

        public static void StopCoroutine(this MonoBehaviour self, Coroutine routine)
        {
            MelonCoroutines.Stop(routine);
        }
    }
}
