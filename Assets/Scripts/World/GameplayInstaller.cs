using System;
using Asteroids.Player;
using Asteroids.Signals;
using Asteroids.UI;
using MessagePipe;
using UnityEngine;
using ZenExtended;
using Zenject;
using Random = System.Random;

namespace Asteroids.World
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private bool _messagePipeStacktrace;
        [SerializeField] private int _randomSeed;
        [SerializeField] private PlayerScoreData _scoreData;
        [SerializeField, InjectOptional] private WorldSettings _settings;
        
        public override void InstallBindings()
        {
            if (_randomSeed == 0)
                _randomSeed = (int) DateTime.UtcNow.Ticks;
            
            Container.BindInstance(new Random(_randomSeed));
            Container.BindInstance(_settings);
            Container.BindInstance(Camera.main);

            Container.Bind<PlayerFacade>()
                .FromComponentInHierarchy()
                .AsSingle();

            RegisterSpawners();
            RegisterMessagePipe();
            RegisterUI();

            Container.BindInstance(_scoreData);
            Container.QueueForInject(_scoreData);
        }

        private void RegisterUI()
        {
            Container.BindInterfacesAndSelfTo<PlayerScoreView>().FromComponentInHierarchy().AsSingle();
            Container.BindPresenter<PlayerScoreView, PlayerScorePresenter>();
        }

        private void RegisterSpawners()
        {
            Container.BindInterfacesAndSelfTo<AsteroidSpawner>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<EnemyShipSpawner>()
                .AsSingle()
                .NonLazy();
        }

        private void RegisterMessagePipe()
        {
            MessagePipeOptions messagePipe = Container.BindMessagePipe(options =>
            {
                if (_messagePipeStacktrace)
                    options.EnableCaptureStackTrace = true;

                options.InstanceLifetime = InstanceLifetime.Singleton;
            });

            Container.BindMessageBroker<ScoreUpdated>(messagePipe);
        }
    }
}
