using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Runtime
{
    public class CameraController : Dummy
    {
        public float followSpeed = 2f;
        public float yOffset = 1f;
        public Transform target;

        public override void OnUpdate(float deltaTime)
        {
            Vector3 newPos = new Vector3(this.target.position.x, this.target.position.y + this.yOffset, - 10f);
            transform.position = Vector3.Slerp(transform.position, newPos, this.followSpeed * Time.deltaTime);
        }
    }
}
