using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Asteroids.World
{
    public class PrefabsInstaller : MonoInstaller
    {
        [FormerlySerializedAs("PlayerBullet")] [FormerlySerializedAs("Bullet")] [FormerlySerializedAs("Prefab")]
        public PlayerBullet PlayerBulletPrefab;

        public EnemyBullet EnemyBulletPrefab;

        public EnemyShip EnemyShip;

        public override void InstallBindings()
        {
            RegisterBulletPool<PlayerBullet, PlayerBullet.Factory>(PlayerBulletPrefab);
            RegisterBulletPool<EnemyBullet, EnemyBullet.Factory>(EnemyBulletPrefab);

            Container.BindFactory<EnemyShip, EnemyShip.Factory>()
                .FromComponentInNewPrefab(EnemyShip);
        }

        private void RegisterBulletPool<T, TFactory>(Bullet<T> prefab)
            where T : Component, IPoolable<BulletSpawnParams, IMemoryPool>
            where TFactory : PlaceholderFactory<BulletSpawnParams, T>
        {
            Container.BindFactory<BulletSpawnParams, T, TFactory>()
                .FromMonoPoolableMemoryPool(x => x
                    .WithInitialSize(30)
                    .FromComponentInNewPrefab(prefab)
                    .UnderTransformGroup("Bullet Pool")
                );
        }
    }
}