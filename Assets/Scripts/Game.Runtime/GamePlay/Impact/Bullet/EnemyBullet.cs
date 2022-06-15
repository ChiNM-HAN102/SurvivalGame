using System;
using Lean.Pool;
using UnityEngine;

namespace Game.Runtime
{
    public class EnemyBullet : MonoBehaviour
    {
        [SerializeField] private EnemyImpact impact;
        
        private Rigidbody2D _rb;
        private Unit _owner;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void InitData(Unit owner, Vector3 force)
        {
            this._owner = owner;
            _rb.AddForce(force);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var impact = LeanPool.Spawn(this.impact, transform.position, Quaternion.identity);
            impact.InitData(this._owner, 0.2f);
            LeanPool.Despawn(gameObject);
        }
    }
}