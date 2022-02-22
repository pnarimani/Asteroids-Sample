using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Asteroids.UI
{
    public interface IPlayerScoreView
    {
        string ScoreText { get; set; }
    }

    public class PlayerScoreView : MonoBehaviour, IPlayerScoreView
    {
         [SerializeField] private TextMeshProUGUI _scoreText;
        
        private PlayerScorePresenter _presenter;

        public string ScoreText
        {
            get => _scoreText.text;
            set => _scoreText.text = value;
        }
        
        [Inject]
        public void Init(PlayerScorePresenter presenter)
        {
            _presenter = presenter;
        }

        private void OnEnable()
        {
            _presenter.OnEnable();
        }

        private void OnDisable()
        {
            _presenter.OnDisable();
        }
    }
}