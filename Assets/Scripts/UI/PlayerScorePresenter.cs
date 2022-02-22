using System;
using Asteroids.Signals;
using MessagePipe;

namespace Asteroids.UI
{
    public class PlayerScorePresenter
    {
        private IPlayerScoreView _view;
        private ISubscriber<ScoreUpdated> _scoreUpdated;
        private IDisposable _eventHandle;

        public PlayerScorePresenter(IPlayerScoreView view, ISubscriber<ScoreUpdated> scoreUpdated)
        {
            _scoreUpdated = scoreUpdated;
            _view = view;
        }

        public void OnEnable()
        {
            _eventHandle = _scoreUpdated.Subscribe(OnScoreUpdated);
            _view.ScoreText = "Score: 0";
        }

        public void OnDisable()
        {
            _eventHandle?.Dispose();
            _eventHandle = null;
        }

        private void OnScoreUpdated(ScoreUpdated obj)
        {
            _view.ScoreText = "Score: " + obj.FinalCount;
        }
    }
}