using System;

namespace Asteroids.World
{
    public class AsteroidNotFoundException : Exception
    {
        public AsteroidNotFoundException(string id) : base($"Could not find asteroid with id `{id}`")
        {
            
        }
    }
}