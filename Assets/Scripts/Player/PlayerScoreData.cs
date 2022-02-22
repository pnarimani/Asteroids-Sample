using Asteroids.Signals;
using JetBrains.Annotations;
using MessagePipe;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Asteroids.Player
{
    [CreateAssetMenu(menuName = "Asteroid/Score Data", fileName = "PlayerScoreData", order = 0)]
    public class PlayerScoreData : ScriptableObject
    {
        [SerializeField] private int _score;
        
        private IPublisher<ScoreUpdated> _scoreUpdated;

        public int Score
        {
            get => _score;
            set
            {
                if (_score == value)
                    return;

                int change = value - _score;
                _score = value;
                _scoreUpdated.Publish(new ScoreUpdated(change, _score));
            }
        }

        [Inject]
        public void Init(IPublisher<ScoreUpdated> scoreUpdated)
        {
            _scoreUpdated = scoreUpdated;
            _score = 0;
        }

        [Button, UsedImplicitly]
        private void PublishScoreUpdated()
        {
            if (!Application.isPlaying)
            {
                Debug.LogError("Publish ScoreUpdated is only available in play mode");
                return;
            }
            
            _scoreUpdated?.Publish(new ScoreUpdated(0, _score));
        }
    }
}