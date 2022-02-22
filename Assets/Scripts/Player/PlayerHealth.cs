using Asteroids.World;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroids.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        public void Damage()
        {
            // Health system and respawn could be added here. But for now, we simply reload
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
