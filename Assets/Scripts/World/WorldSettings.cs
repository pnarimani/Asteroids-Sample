using UnityEngine;

namespace Asteroids.World
{
    [CreateAssetMenu(fileName = "WorldSettings", menuName = "Asteroids/WorldSettings", order = 0)]
    public class WorldSettings : ScriptableObject
    {
        public float EnemyShipSpawnInterval = 7;
        public string FirstAsteroidId;
        public int AsteroidStartCount = 3;
        public float BulletSpeed = 10;
        public float BulletLifetime = 10;
    }
}