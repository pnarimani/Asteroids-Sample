using System.Collections.Generic;
using Zenject;

namespace Asteroids.World
{
    public class AsteroidInstaller : MonoInstaller
    {
        public List<Asteroid> Asteroids;

        public override void InstallBindings()
        {
            // We have Asteroid.Factory which other classes can use to spawn asteroid.
            // The problem is that each asteroid has a separate prefab and it is not possible to bind multiple prefabs to one factory in Zenject.
            // The solution here is to have an intermediate factory (PooledAsteroidFactory) for every prefab.
            // Every time a new asteroid instance is requested, we resolve the intermediate factory first,
            // then use the intermediate factory to spawn the actual asteroid. 
            
            foreach (Asteroid z in Asteroids)
            {
                Container.BindFactory<Asteroid, PooledAsteroidFactory>()
                    .WithId(z.Id)
                    .FromMonoPoolableMemoryPool(x => x
                        .FromComponentInNewPrefab(z)
                        .UnderTransformGroup("Asteroid Pool " + z.Id)
                    );
            }

            Container.BindFactory<string, Asteroid, Asteroid.Factory>()
                .FromMethod(Spawn);
        }

        private static Asteroid Spawn(DiContainer container, string id)
        {
            if (!container.HasBindingId<PooledAsteroidFactory>(id))
                throw new AsteroidNotFoundException(id);
            
            return container.ResolveId<PooledAsteroidFactory>(id).Create();
        }

        private class PooledAsteroidFactory : PlaceholderFactory<Asteroid>
        {
        }
    }
}