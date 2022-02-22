using UnityEngine;
using ZenExtended;
using Zenject;

namespace Asteroids.World
{
    public class EnemyShipSpawner : ITickable
    {
        private EnemyShip.Factory _enemyFactory;
        private float _nextSpawnTime;
        private WorldSettings _worldSettings;
        private Camera _camera;

        [Inject]
        public void Init(EnemyShip.Factory enemyFactory, WorldSettings worldSettings, Camera camera)
        {
            _camera = camera;
            _worldSettings = worldSettings;
            _enemyFactory = enemyFactory;
        }


        public void Tick()
        {
            if (Time.time > _nextSpawnTime)
            {
                _nextSpawnTime = Time.time + _worldSettings.EnemyShipSpawnInterval;
                SpawnShip();
            }
        }

        private void SpawnShip()
        {
            Vector3 spawnPos = _camera.ViewportToWorldPoint(new Vector3(0,0.5f,0));
            EnemyShip ship = _enemyFactory.Create();
            ship.Position = spawnPos;
        }
    }
}