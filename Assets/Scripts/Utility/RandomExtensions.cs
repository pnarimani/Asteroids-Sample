using UnityEngine;
using Random = System.Random;

namespace Asteroids.Utility
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random random, float min, float max)
        {
            return (float) (random.NextDouble() * (max - min) + min);
        }

        public static Vector2 NextUniformCircle(this Random random)
        {
            return new Vector2(
                random.NextFloat(-1, 1),
                random.NextFloat(-1, 1)
            ).normalized;
        }
    }
}