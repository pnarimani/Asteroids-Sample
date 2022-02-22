using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    [RequireComponent(typeof(TeleportingEntity))]
    public class TeleportingTrail : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trail;
        private bool _clearTrail;

        private void Start()
        {
            var t = GetComponent<TeleportingEntity>();
            t.BeforeTeleport += OnBeforeTeleport;
            t.AfterTeleport += OnAfterTeleport;
        }

        private void OnEnable()
        {
            _trail.gameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            if (_clearTrail)
            {
                _trail.Clear();
                _trail.SetPositions(Array.Empty<Vector3>());
                _trail.gameObject.SetActive(true);
                _clearTrail = false;
            }
        }

        private void OnBeforeTeleport()
        {
            TrailRenderer cloneTrail = Instantiate(_trail, _trail.transform.parent, false);
            cloneTrail.name = "Trail";
            _trail.transform.parent = null;
            Destroy(_trail.gameObject, 3);
            _trail = cloneTrail;
            _trail.gameObject.SetActive(false);
        }

        private void OnAfterTeleport()
        {
            _clearTrail = true;
        }
    }
}