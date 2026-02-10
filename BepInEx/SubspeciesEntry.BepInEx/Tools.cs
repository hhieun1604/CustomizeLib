using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SubspeciesEntry.BepInEx
{
    public static class Tools
    {
        /// <summary>
        /// 获取嵌入dll里的ab包
        /// </summary>
        /// <param name="assembly">要获取ab包的dll</param>
        /// <param name="name">名称</param>
        /// <returns>ab包</returns>
        /// <exception cref="ArgumentException"></exception>
        public static AssetBundle GetAssetBundle(Assembly assembly, string name)
        {
            try
            {
                using Stream stream =
                    assembly.GetManifestResourceStream(assembly.FullName!.Split(",")[0] + "." + name) ??
                    assembly.GetManifestResourceStream(name)!;
                using MemoryStream stream1 = new();
                stream.CopyTo(stream1);
                var ab = AssetBundle.LoadFromMemory(stream1.ToArray());
                ArgumentNullException.ThrowIfNull(ab);
                return ab;
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Failed to load {name} \n{e}");
            }
        }

        public static T GetAsset<T>(this AssetBundle ab, string name) where T : UnityEngine.Object
        {
            foreach (var ase in ab.LoadAllAssetsAsync().allAssets)
            {
                if (ase.TryCast<T>()?.name == name)
                {
                    return ase.Cast<T>();
                }
            }
            throw new ArgumentException($"Could not find {name} from {ab.name}");
        }
    }
}
