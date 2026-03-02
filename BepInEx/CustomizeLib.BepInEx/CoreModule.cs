using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomizeLib.BepInEx
{
    public class CoreBehaviour : MonoBehaviour
    {
        public static CoreBehaviour? Instance = null;

        public void Awake()
        {
            if (Instance != null)
                Destroy(this.gameObject);
            Instance = this;
        }

        public void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    // PatchMgr.ReloadSkin();
                }
            }
        }
    }
}
