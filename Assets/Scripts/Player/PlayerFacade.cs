using UnityEngine;
using Zenject;

namespace Asteroids.Player
{
    /// <summary>
    /// A class which represents Player. Classes outside Player context will use this class to interact with the player.
    /// </summary>
    /// <see>
    ///     <cref>https://www.dofactory.com/net/facade-design-pattern</cref>
    /// </see>
    public class PlayerFacade : MonoBehaviour
    {
        private PlayerMovement _playerMovement;

        public Vector2 Position => _playerMovement.Position;

        [Inject]
        public void Init(PlayerMovement playerMovement)
        {
            _playerMovement = playerMovement;
        }
    }
}