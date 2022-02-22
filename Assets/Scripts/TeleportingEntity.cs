using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace Asteroids
{
    public class TeleportingEntity : MonoBehaviour
    {
        private Camera _camera;
        private Rigidbody2D _rigidbody;

        // A flag to prevent teleporting the same object infinite times.
        private bool _isEnabled;

        public event Action BeforeTeleport, AfterTeleport;

        private void Awake()
        {
            _camera = Camera.main;
            _rigidbody = GetComponent<Rigidbody2D>();

            // If the gameObject which this component is attached to doesn't have renderer, We are not going to receive visibility events.
            // The solution is to find a renderer in its children and use that renderer to get the events.
            if (GetComponent<Renderer>() == null)
            {
                var childRenderer = GetComponentInChildren<Renderer>();
                if (childRenderer != null)
                {
                    ListenOnBecameInvisible(childRenderer).Forget();
                    ListenOnBecameVisible(childRenderer).Forget();
                }
                else
                {
                    Debug.LogError(
                        "Could not find Renderer to use with TeleportingEntity. " +
                        "Please make sure a Renderer component is attached to the same object or one of the children of the object.",
                        this);
                }
            }
        }

        private void OnEnable()
        {
            _isEnabled = true;
        }

        private async UniTask ListenOnBecameInvisible(Renderer targetRenderer)
        {
            IAsyncOnBecameInvisibleHandler handler = targetRenderer
                .GetAsyncBecameInvisibleTrigger()
                .GetOnBecameInvisibleAsyncHandler();

            CancellationToken ct = this.GetCancellationTokenOnDestroy();

            while (this != null)
            {
                await handler.OnBecameInvisibleAsync();
                ct.ThrowIfCancellationRequested();
                OnBecameInvisible();
            }
        }

        private async UniTask ListenOnBecameVisible(Renderer targetRenderer)
        {
            IAsyncOnBecameVisibleHandler handler = targetRenderer
                .GetAsyncBecameVisibleTrigger()
                .GetOnBecameVisibleAsyncHandler();

            CancellationToken ct = this.GetCancellationTokenOnDestroy();

            while (this != null)
            {
                await handler.OnBecameVisibleAsync();
                ct.ThrowIfCancellationRequested();
                OnBecameVisible();
            }
        }

        private void OnBecameInvisible()
        {
            if (!gameObject.activeSelf)
                return;
            if (_camera == null)
                return;
            
            Vector3 frustum = _camera.WorldToViewportPoint(transform.position);
            if (frustum.x < 0 || frustum.x > 1)
            {
                _isEnabled = false;
                frustum.x = 1 - Mathf.Clamp01(frustum.x);
            }

            if (frustum.y < 0 || frustum.y > 1)
            {
                _isEnabled = false;
                frustum.y = 1 - Mathf.Clamp01(frustum.y);
            }

            // If the object needs to be teleported, isEnabled will be false
            if (!_isEnabled)
            {
                BeforeTeleport?.Invoke();

                Vector3 targetPos = _camera.ViewportToWorldPoint(frustum);
                if (_rigidbody != null)
                    _rigidbody.MovePosition(targetPos);
                else
                    transform.position = targetPos;
            }
        }

        private void OnBecameVisible()
        {
            // It is sometimes necessary for active status for pooled objects  
            if (!gameObject.activeSelf)
                return;
            
            // If teleporting is not enabled (object has just teleported) and object became visible, it means that teleporting is complete
            if (!_isEnabled)
                AfterTeleport?.Invoke();

            _isEnabled = true;
        }
    }
}