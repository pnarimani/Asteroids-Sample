using System;
using Asteroids.World;
using Audoty;
using UnityEngine;
using ZenExtended;
using Object = UnityEngine.Object;

namespace Asteroids
{
    public abstract class Bullet<T> : MonoSpawnable<BulletSpawnParams, T>
    {
        [SerializeField] private AudioPlayer _hitSFX;
        
        private Rigidbody2D _rigidbody;
        private float _destroyTime;
        private bool _isSpawned;
        private Object _currentOwner;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Time.time >= _destroyTime && _isSpawned)
            {
                _isSpawned = false;
                Dispose();
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_currentOwner == col.gameObject)
                return;

            _hitSFX.Play();

            var damageable = col.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.Damage();
            
            Dispose();
        }

        protected override void OnPoolSpawned(BulletSpawnParams spawnParams)
        {
            transform.position = spawnParams.StartPosition;
            _rigidbody.velocity = spawnParams.Velocity;
            _destroyTime = Time.time + spawnParams.Lifetime;
            _currentOwner = spawnParams.Owner;
            _isSpawned = true;
            ResetTrail();
        }

        protected override void OnPoolDespawned()
        {
            ResetTrail();
        }

        private void ResetTrail()
        {
            // we need to get TrailRenderer every time because it will change to a new trail object every time bullet teleports
            var trail = GetComponentInChildren<TrailRenderer>();
            if (trail != null)
            {
                trail.Clear();
                trail.SetPositions(Array.Empty<Vector3>());
            }
        }
    }

    public readonly struct BulletSpawnParams
    {
        public readonly Object Owner;
        public readonly Vector3 StartPosition, Velocity;
        public readonly float Lifetime;

        public BulletSpawnParams(Object owner, Vector3 startPosition, Vector3 velocity, float lifetime)
        {
            StartPosition = startPosition;
            Velocity = velocity;
            Lifetime = lifetime;
            Owner = owner;
        }
    }
}