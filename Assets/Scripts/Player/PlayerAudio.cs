using Audoty;
using UnityEngine;

namespace Asteroids.Player
{
    public interface IPlayerAudio
    {
        void PlayShoot();
    }

    /// <summary>
    /// PlayerAudio is responsible for playing all the audio that player object plays
    /// </summary>
    public class PlayerAudio : MonoBehaviour, IPlayerAudio
    {
        [SerializeField] private AudioPlayer _shootSFX;

        public void PlayShoot()
        {
            _shootSFX.Play();
        }
    }
}