using Audoty;
using UnityEngine;
using Zenject;

namespace Asteroids.Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField, InjectOptional] private PlayerSettings _settings;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_settings);

            NewComponent<PlayerHealth>().NonLazy();

            // An example of how you would add functionality to a gameobject without having MonoBehaviours
            NewEntryPoint<PlayerMovement>();
            NewEntryPoint<PlayerShooting>();
            
            Container.Bind<PlayerFacade>()
                .FromComponentOnRoot()
                .AsSingle();
            
            // We don't bind PlayerAudio here because we don't want to allow classes to couple directly with PlayerAudio.
            // It is useful to only interact with an interface for visual/audio behaviours because it can be mocked for unit tests.
            Container.Bind<IPlayerAudio>()
                .To<PlayerAudio>()
                .FromComponentOnRoot()
                .AsSingle();

            // It is possible to use different IPlayerInput implementations for different platforms.
            // We can also mock IPlayerInput for unit tests.
            Container.Bind<IPlayerInput>()
                .To<PlayerInputReceiver>()
                .FromNewComponentOnRoot()
                .AsSingle();

            // Bind components which behaviour classes might find useful
            Container.BindInstance(transform);
            Container.BindInstance(GetComponent<Rigidbody2D>());
            Container.BindInstance(GetComponent<CircleCollider2D>());
        }

        private IfNotBoundBinder NewEntryPoint<T>()
        {
            return Container.BindInterfacesAndSelfTo<T>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }

        private ConcreteIdArgConditionCopyNonLazyBinder NewComponent<T>()
        {
            return Container.BindInterfacesAndSelfTo<T>()
                .FromNewComponentOnRoot()
                .AsSingle();
        }
    }
}