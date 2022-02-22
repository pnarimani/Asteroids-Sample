using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace Asteroids.World
{
    public class GameBorder : MonoBehaviour
    {
        [SerializeField] private float _borderWidth = 0.1f;
        [SerializeField] private bool _generateBorders;
        [SerializeField] private BoxCollider2D _top, _left, _right, _bottom;


        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            if (_generateBorders)
            {
                Vector3 topLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0));
                Vector3 botLeft = _camera.ViewportToWorldPoint(new Vector3(0, 1));
                Vector3 botRight = _camera.ViewportToWorldPoint(new Vector3(1, 1));
                Vector3 topRight = _camera.ViewportToWorldPoint(new Vector3(1, 0));

                _top = CreateBorder(topLeft, topRight, "Top Border");
                _left = CreateBorder(topLeft, botLeft, "Left Border");
                _right = CreateBorder(topRight, botRight, "Right Border");
                _bottom = CreateBorder(botLeft, botRight, "Bottom Border");
            }

            LinkColliders(_top, _bottom).Forget();
            LinkColliders(_bottom, _top).Forget();
            LinkColliders(_left, _right).Forget();
            LinkColliders(_right, _left).Forget();
        }

        private BoxCollider2D CreateBorder(Vector3 from, Vector3 to, string borderName)
        {
            var c = new GameObject(borderName, typeof(BoxCollider2D))
                .GetComponent<BoxCollider2D>();
            c.isTrigger = true;

            Vector3 dir = (to - from);
            c.size = new Vector2(dir.magnitude, _borderWidth);

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            c.transform.SetPositionAndRotation(
                (@from + to) / 2,
                Quaternion.AngleAxis(angle, Vector3.forward)
            );

            return c;
        }

        private static async UniTask LinkColliders(BoxCollider2D srcCol, BoxCollider2D destCol)
        {
            IAsyncOnTriggerEnter2DHandler handler = srcCol
                .GetAsyncTriggerEnter2DTrigger()
                .GetOnTriggerEnter2DAsyncHandler();

            while (Application.isPlaying && srcCol != null)
            {
                Collider2D other = await handler.OnTriggerEnter2DAsync();

                Physics2D.IgnoreCollision(srcCol, other);
                Physics2D.IgnoreCollision(destCol, other);
                RemoveIgnoreColliders(srcCol, other, 0.5f).Forget();
                RemoveIgnoreColliders(destCol, other, 0.5f).Forget();

                Transform objTrans = other.transform;
                Vector3 offset = objTrans.position - srcCol.transform.position;
                Vector3 destPos = destCol.transform.position;
                objTrans.position = destPos + offset;
            }
        }

        private static async UniTask RemoveIgnoreColliders(Collider2D c1, Collider2D c2, float time)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(time));
            Physics2D.IgnoreCollision(c1, c2, false);
        }
    }
}