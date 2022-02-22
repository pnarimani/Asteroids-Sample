using System;
using Asteroids.World;
using UnityEngine;
using ZenExtended;
using Zenject;

namespace Asteroids.Player
{
    public class PlayerShooting : IInitializable
    {
        private PlayerBullet.Factory _bulletFactory;
        private PlayerInputReceiver _input;
        private PlayerSettings _playerSettings;
        private Rigidbody2D _rigidbody;
        private WorldSettings _worldSettings;
        private Transform _transform;

        private float _nextShootTime;
        private IPlayerAudio _audio;

        public event Action BulletShot;

        [Inject]
        public void Init(
            PlayerBullet.Factory bulletFactory, 
            PlayerInputReceiver input,
            PlayerSettings playerSettings,
            Rigidbody2D rb,
            WorldSettings worldSettings,
            Transform transform,
            IPlayerAudio audio)
        {
            _audio = audio;
            _transform = transform;
            _worldSettings = worldSettings;
            _rigidbody = rb;
            _playerSettings = playerSettings;
            _input = input;
            _bulletFactory = bulletFactory;
        }

        public void Initialize()
        {
            _input.Shoot += InputOnShoot;
        }

        private void InputOnShoot()
        {
            if (_nextShootTime > Time.time)
                return;
            _nextShootTime = Time.time + (60f / _playerSettings.FireRate);

            Vector3 speed = _transform.up * (_worldSettings.BulletSpeed);

            float dot = Vector2.Dot(_rigidbody.velocity.normalized, speed.normalized);
            speed += (Vector3) (Mathf.Max(dot, 0) * _rigidbody.velocity);

            _bulletFactory.Create(new BulletSpawnParams(_transform, _transform.position, speed, _worldSettings.BulletLifetime));

            _audio.PlayShoot();
            BulletShot?.Invoke();
        }
    }
}