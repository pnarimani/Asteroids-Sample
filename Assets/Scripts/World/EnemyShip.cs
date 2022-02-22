using System;
using Asteroids.Player;
using UnityEngine;
using ZenExtended;
using Zenject;

namespace Asteroids.World
{
    public class EnemyShip : MonoSpawnable<EnemyShip>, IDamageable
    {
        private PlayerScoreData _scoreData;
        private Rigidbody2D _rigidbody;
        private MonoSpawnable<BulletSpawnParams, EnemyBullet>.Factory _bulletFactory;
        private PlayerFacade _playerFacade;
        private WorldSettings _worldSettings;

        public Vector2 Position
        {
            get => _rigidbody.position;
            set
            {
                transform.position = value;
                _rigidbody.MovePosition(value);
            }
        }

        [Inject]
        public void Init(
            PlayerScoreData scoreData,
            EnemyBullet.Factory bulletFactory,
            PlayerFacade playerFacade,
            WorldSettings worldSettings)
        {
            _worldSettings = worldSettings;
            _playerFacade = playerFacade;
            _bulletFactory = bulletFactory;
            _scoreData = scoreData;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(ShootBullet), 3, 3);
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(1, 0);
        }

        private void ShootBullet()
        {
            Vector2 dir = _playerFacade.Position - Position;
            _bulletFactory.Create(new BulletSpawnParams(
                this,
                Position,
                dir.normalized * _worldSettings.BulletSpeed,
                _worldSettings.BulletLifetime)
            );
        }

        public void Damage()
        {
            _scoreData.Score += 10;
            Dispose();
        }
    }
}