using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrickBreaker
{
    /// <summary>
    /// The player paddle. Handles player input and movement.
    /// </summary>
    public class Paddle : NetworkBehaviour
    {
        /// <summary>The paddle's collider.</summary>
        [Header("References")]
        [SerializeField] private Collider2D col;


        /// <summary>The paddle's horizontal movement speed.</summary>
        [Header("Properties")]
        [SerializeField] private float speed;
        /// <summary>The minimum and maximum x positions that the paddle can move to.</summary>
        [SerializeField] private Vector2 xMinMax;


        /// <summary>The y position of the top paddle.</summary>
        private static float topPaddleHeight = -3;
        /// <summary>The y position of the bottom paddle.</summary>
        private static float bottomPaddleHeight = -4;

        private void Update()
        { 
            if (isLocalPlayer)
            {
                // Calls the Fire command when the Fire button is pressed
                if (Input.GetButtonDown("Fire"))
                {
                    CmdFire();
                }
            }
        }

        void FixedUpdate()
        {
            if (isLocalPlayer)
            {
                // Moves the paddle left and right base on the Horizontal input axis and the paddle's speed
                float move = Input.GetAxisRaw("Horizontal") * speed;
                float xPos = Mathf.Clamp(transform.localPosition.x + move * Time.fixedDeltaTime, xMinMax.x, xMinMax.y);
                transform.localPosition = new Vector3(xPos, transform.localPosition.y, transform.localPosition.z);
            }
        }

        /// <summary>
        /// Called by when the client presses the Fire key. Fires the ball if the player is the top player.
        /// </summary>
        /// <seealso cref="GameManager.FireBall"/>
        [Command]
        void CmdFire()
        {
            if (GameManager.TopPlayer() == this)
            {
                GameManager.FireBall();
            }
        }

        /// <summary>
        /// Called from server to move the paddle to the top position.
        /// </summary>
        [TargetRpc]
        public void TargetMoveTop()
        {
            transform.localPosition = new Vector3(transform.localPosition.x, topPaddleHeight, transform.localPosition.z);
        }

        /// <summary>
        /// Called from server to move the paddle to the bottom position.
        /// </summary>
        [TargetRpc]
        public void TargetMoveBottom()
        {
            transform.localPosition = new Vector3(transform.localPosition.x, bottomPaddleHeight, transform.localPosition.z);
        }

        /// <summary>
        /// Returns the top paddle height.
        /// </summary>
        public static float GetTopPaddleHeight()
        {
            return topPaddleHeight;
        }

        /// <summary>
        /// Returns the bottom paddle height.
        /// </summary>
        public static float GetBottomPaddleHeight()
        {
            return bottomPaddleHeight;
        }
    }
}