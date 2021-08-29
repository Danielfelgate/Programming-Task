using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace BrickBreaker
{
    /// <summary>
    /// Handles breaking bricks when collided by the ball, increasing the score and playing particle effects.
    /// </summary>
    public class Brick : NetworkBehaviour
    {
        /// <summary>The brick's <see cref="Collider2D"/></summary>
        [Header("References")]
        [SerializeField] private Collider2D col;
        /// <summary>The brick's <see cref="SpriteShapeRenderer"/></summary>
        [SerializeField] private SpriteShapeRenderer rend;
        /// <summary>The brick's <see cref="ParticleSystem"/></summary>
        [SerializeField] private ParticleSystem part;


        /// <summary>
        /// Called on the server when the <see cref="Ball"/> collides with the brick. 
        /// Invokes visual break on clients and disables the collider, increases the score, and queues the game object destruction on the server.
        /// </summary>
        [Server]
        public void Break()
        {
            RpcBreakVisual();

            col.enabled = false;
            GameManager.IncreaseScore();
            StartCoroutine("DelayedDestroy", 2);
        }

        /// <summary>
        /// Called by <see cref="Break"/> to display the brick's destruction on clients.
        /// </summary>
        [ClientRpc]
        public void RpcBreakVisual()
        {
            rend.enabled = false;

            // Sets the particle systems color to match the renderer before playing it
            ParticleSystem.MainModule main = part.main;
            main.startColor = rend.color;
            part.Play();
        }

        /// <summary>
        /// Coroutine for destroying the brick on the server after a delay.
        /// </summary>
        /// <param name="seconds">The number of seconds to wait before destroying the brick.</param>
        /// <returns></returns>
        [Server]
        IEnumerator DelayedDestroy(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            NetworkServer.Destroy(gameObject);
            yield return null;
        }
    }
}
