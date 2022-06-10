using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class BehaviorTreeRunner : MonoBehaviour
    {
        private BehaviorTree tree;

        private void Start()
        {
            tree = ScriptableObject.CreateInstance<BehaviorTree>();

            var log = ScriptableObject.CreateInstance<DebugLogNode>();

            log.message = "HELLLOOO 1";

            var pause = ScriptableObject.CreateInstance<WaitNode>();
            pause.duration = 5;
            
            var log2 = ScriptableObject.CreateInstance<DebugLogNode>();

            log2.message = "HELLLOOO 2";
            
            var log3 = ScriptableObject.CreateInstance<DebugLogNode>();

            log3.message = "HELLLOOO 3";

            var sequence = ScriptableObject.CreateInstance<SequenceNode>();
            sequence.children.Add(log);
            sequence.children.Add(pause);
            sequence.children.Add(log2);
            sequence.children.Add(log3);

            var loop = ScriptableObject.CreateInstance<RepeatNode>();

            loop.child = sequence;
            this.tree.rootNode = loop;
        }

        private void Update()
        {
            this.tree.Update(Time.deltaTime);
        }
    }
}
