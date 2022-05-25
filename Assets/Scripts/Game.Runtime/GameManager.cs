using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            InitGlobalUpdateSystem();
        }

        void InitGlobalUpdateSystem()
        {
            DontDestroyOnLoad(GlobalUpdateSystem.Instance.gameObject);
        }
    }
}
