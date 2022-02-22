using System;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Asteroids.World
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private float _checkInterval = 1;

        private Asteroid.Factory _asteroidFactory;

        private int _waveNumber;
        private float _nextCheckTime;
        private Camera _camera;
        private Random _random;
        private WorldSettings _worldSettings;

        [Inject]
        public void Init(Asteroid.Factory asteroidFactory, Random random, WorldSettings worldSettings)
        {
            _worldSettings = worldSettings;
            _random = random;
            _asteroidFactory = asteroidFactory;
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Time.time > _nextCheckTime)
            {
                _nextCheckTime = Time.time + _checkInterval;

                if (Asteroid.ActiveAsteroids <= 0)
                {
                    SpawnNextWave();
                }
            }
        }

        private void SpawnNextWave()
        {
            int numToSpawn = _worldSettings.AsteroidStartCount + _waveNumber;
            _waveNumber++;

            for (int i = 0; i < numToSpawn; i++)
            {
                Asteroid asteroid = _asteroidFactory.Create(_worldSettings.FirstAsteroidId);

                bool spawnOnLongSide = _random.NextDouble() < (float) Screen.width / (Screen.width + Screen.height);

                Vector3 spawnPos = _camera.ViewportToWorldPoint(
                    spawnOnLongSide
                        ? new Vector3((float) _random.NextDouble(), _random.Next(0, 2), 0)
                        : new Vector3(_random.Next(0, 2), (float) _random.NextDouble(), 0)
                );

                asteroid.Position = spawnPos;
                asteroid.RandomizeRotationAndVelocity(asteroid.InitialVelocityMagnitude);
            }
        }
    }
}