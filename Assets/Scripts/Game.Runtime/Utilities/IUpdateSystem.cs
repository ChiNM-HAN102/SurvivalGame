using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public interface IUpdateSystem
    {
    /// <summary>
    /// Do action when update call
    /// </summary>
    /// <param name="deltaTime"></param>
    void OnUpdate(float deltaTime);
    }
}
