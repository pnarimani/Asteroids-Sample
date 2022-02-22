using UnityEngine;

namespace Asteroids.Player
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Asteroids/PlayerSettings", order = 0)]
    public class PlayerSettings : ScriptableObject
    {
        public float AccellerationMultiplier = 5;
        public float DecelerationOverTime = 1;
        public float ShootingRecoil = 0.2f;
        public int FireRate = 200;

    }
}