using UnityEngine;
using Zenject;

namespace Asteroids.Player
{
    public class PlayerMovement : IInitializable
    {
        private PlayerSettings _settings;
        private PlayerInputReceiver _input;
        private Rigidbody2D _rigidbody2D;
        private PlayerShooting _shooting;
        private Transform _transform;

        public Vector2 Position
        {
            get => _rigidbody2D.position;
            set
            {
                _transform.position = value;
                _rigidbody2D.MovePosition(value);
            }
        }

        public float Rotation
        {
            get => _rigidbody2D.rotation;
            set => _rigidbody2D.rotation = value;
        }
        
        [Inject]
        public void Init(PlayerSettings settings, PlayerInputReceiver input, Rigidbody2D rb, PlayerShooting shooting, Transform transform)
        {
            _transform = transform;
            _shooting = shooting;
            _rigidbody2D = rb;
            _input = input;
            _settings = settings;
        }

        public void Initialize()
        {
            _input.LookDirection += InputOnLookDirection;
            _input.Accelerate += InputOnAccelerate;

            _shooting.BulletShot += InputOnShoot;
        }

        private void InputOnShoot()
        {
            _rigidbody2D.AddForce(-_transform.up * _settings.ShootingRecoil, ForceMode2D.Impulse);
        }

        private void InputOnLookDirection(Vector2 dir)
        {
            _rigidbody2D.SetRotation(Quaternion.LookRotation(Vector3.forward, dir.normalized));
        }

        private void InputOnAccelerate(float acceleration)
        {
            _rigidbody2D.AddForce(_transform.up * _settings.AccellerationMultiplier * acceleration, ForceMode2D.Impulse);
        }
    }
}