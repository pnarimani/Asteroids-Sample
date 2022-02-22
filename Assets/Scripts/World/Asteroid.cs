using System;
using System.Collections.Generic;
using Asteroids.Player;
using Asteroids.Utility;
using UnityEngine;
using ZenExtended;
using Zenject;
using Random = System.Random;

namespace Asteroids.World
{
    public class Asteroid : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable, IDamageable
    {
        [SerializeField] private string _id;
        [SerializeField] private float _initialVelocityMagnitude = 30;
        [SerializeField] private List<string> _spawnOnDestroy;

        private Rigidbody2D _rigidbody;
        private Factory _factory;
        private IMemoryPool _pool;
        private Random _random;
        private PlayerScoreData _playerScoreData;

        public string Id => _id;

        public static int ActiveAsteroids { get; private set; }

        public float InitialVelocityMagnitude => _initialVelocityMagnitude;

        public Vector2 Position
        {
            get => _rigidbody.position;
            set
            {
                transform.position = value;
                _rigidbody.MovePosition(value);
            }
        }

        public float Rotation
        {
            get => _rigidbody.rotation;
            set => _rigidbody.rotation = value;
        }

        [Inject]
        public void Init(Factory factory, Random random, PlayerScoreData playerScoreData)
        {
            _playerScoreData = playerScoreData;
            _random = random;
            _factory = factory;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Damage()
        {
            foreach (string id in _spawnOnDestroy)
            {
                Asteroid obj = _factory.Create(id);
                obj.Position = Position;
                obj.RandomizeRotationAndVelocity(InitialVelocityMagnitude);
            }

            _playerScoreData.Score++;
            Dispose();
        }

        public void OnSpawned(IMemoryPool p1)
        {
            _pool = p1;
            ActiveAsteroids++;
        }

        public void OnDespawned()
        {
            ActiveAsteroids--;
        }

        public void Dispose()
        {
            _pool?.Despawn(this);
            _pool = null;
        }

        public void AddForce(Vector2 force)
        {
            _rigidbody.AddForce(force);
        }

        public void RandomizeRotationAndVelocity(float velocityMagnitude)
        {
            _rigidbody.MoveRotation(_random.Next(0, 360));
            AddForce(_random.NextUniformCircle() * velocityMagnitude);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            var damageable = col.gameObject.GetComponent<IDamageable>();
            if(damageable != null)
                damageable.Damage();
        }

        public class Factory : PlaceholderFactory<string, Asteroid>
        {
        }
    }
}