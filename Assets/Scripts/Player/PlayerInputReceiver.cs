using System;
using UnityEngine;

namespace Asteroids.Player
{
    public delegate void LookDirectionHandler(Vector2 direction);

    public delegate void ShootHandler();

    public delegate void AccelerateHandler(float acceleration);

    public class PlayerInputReceiver : MonoBehaviour
    {
        private Camera _camera;

        public event LookDirectionHandler LookDirection;
        public event ShootHandler Shoot;
        public event AccelerateHandler Accelerate;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = mousePos - transform.position;

            if (dir.sqrMagnitude > 0)
                OnLookDirection(dir.normalized);

            if (Input.GetMouseButton(0))
                OnShoot();

            if (Input.GetKey(KeyCode.W))
                OnAccelerate(1);

            if (Input.GetKey(KeyCode.S))
                OnAccelerate(-1);
        }

        protected void OnLookDirection(Vector2 dir)
        {
            LookDirection?.Invoke(dir);
        }

        protected void OnShoot()
        {
            Shoot?.Invoke();
        }

        protected void OnAccelerate(float acceleration)
        {
            Accelerate?.Invoke(acceleration);
        }
    }
}