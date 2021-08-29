using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrickBreaker
{

    /// <summary>
    /// Controls the server side ball simulation.
    /// </summary>
    public class Ball : NetworkBehaviour
    {
        /// <summary>The ball's rigidbody component.</summary>
        [Header("References")]
        [SerializeField] private Rigidbody2D rb;


        /// <summary>The ball's speed. It's velocity will be forced to this magnitude whenever it is moving.</summary>
        [Header("Properties")]
        [SerializeField] private float speed = 10;
        /// <summary>The offset of the ball's position from the serving paddle's position</summary>
        [SerializeField] private float paddleOffset = 0.4f;


        /// <summary>Tracks whether the ball is being held to a player's paddle</summary>
        private bool serving = true;

        [ServerCallback]
        private void FixedUpdate()
        {
            // Fix the ball's velocity to the set speed if it has any velocity
            if (rb.velocity.magnitude > Mathf.Epsilon)
            {
                rb.velocity = rb.velocity.normalized * speed;
            }

            // If a player is serving then fix the ball's position to the paddle
            if (serving) transform.position = GameManager.TopPlayer().transform.position + Vector3.up * paddleOffset;
        }

        [ServerCallback]
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Shake the camera on every ball collision
            GameManager.ShakeCam();

            // If the ball collides with a brick then destroy it
            if (collision.gameObject.CompareTag("Brick"))
            {
                Brick brick = collision.gameObject.GetComponent<Brick>();
                if (brick)
                {
                    brick.Break();
                }
            }
            // If the ball colliders with the destroyer block then reset the ball to be served
            else if (collision.gameObject.CompareTag("Destroyer"))
            {
                GameManager.ResetBall();
                rb.simulated = false;
                serving = true;
            }
        }

        /// <summary>
        /// Fires the ball and begins simulating it's rigidbody.
        /// </summary>
        [Server]
        public void Fired()
        {
            if (serving)
            {
                serving = false;
                rb.simulated = true;
                rb.velocity = new Vector2(Random.value * 2 - 1, 1);
            }
        }
    }
}