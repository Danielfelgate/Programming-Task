using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrickBreaker
{
    /// <summary>
    /// Handles tracking the players, score, and ball as well as the game functions.
    /// </summary>
    public class GameManager : NetworkBehaviour
    {
        /// <summary>Singleton instance.</summary>
        private static GameManager _instance;

        /// <summary>Displays the score.</summary>
        [Header("References")]
        [SerializeField] private Text scoreText;


        /// <summary>The increase in score when a block is broken.</summary>
        [Header("Properties")]
        [SerializeField] private int scoreIncrease = 100;


        /// <summary>The current game score. Uses <see cref="SyncVarAttribute"/> to synchronize its value to clients and call the <see cref="UpdateScoreText"/> callback.</summary>
        [SyncVar(hook = nameof(UpdateScoreText))] private int score = 0;
        
        /// <summary>List of currently joined players.</summary>
        private List<Paddle> players = new List<Paddle>();
        /// <summary>Index in <see cref="players"/> of the player that is in the top position.</summary>
        private int topPlayer = 0;
        
        /// <summary>Reference to the spawned ball.</summary>
        private Ball ball;

        void Start()
        {
            // Remove existing instance to ensure singleton pattern
            if (_instance) Destroy(_instance.gameObject);
            _instance = this;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            // Display the score text when joining a game that has already progressed
            scoreText.text = "Score: " + score;
        }

        /// <summary>
        /// Increases the current game score (<see cref="score"/>) by <see cref="scoreIncrease"/>.
        /// </summary>
        [Server]
        public static void IncreaseScore()
        {
            _instance.score += _instance.scoreIncrease;
        }

        /// <summary>
        /// Callback for the <see cref="score"/> SyncVar. Updates the score text to display the current score.
        /// </summary>
        public void UpdateScoreText(int oldScore, int newScore)
        {
            scoreText.text = "Score: " + newScore;
        }

        /// <summary>
        /// Resets the score text to it's default value.
        /// </summary>
        public static void ResetScoreText()
        {
            if (_instance) _instance.scoreText.text = "Score: 0";
        }

        /// <summary>
        /// Adds a player to <see cref="players"/> list.
        /// </summary>
        [Server]
        public static void AddPlayer(Paddle player)
        {
            _instance.players.Add(player);
        }

        /// <summary>
        /// Removes a player from the <see cref="players"/> list and makes the remaining player the top player if there is one.
        /// </summary>
        [Server]
        public static void RemovePlayer(Paddle player)
        {
            _instance.players.Remove(player);
            if (_instance.players.Count > 0) _instance.players[0].TargetMoveTop();
            _instance.topPlayer = 0;
        }
        
        /// <summary>
        /// Called when the ball hits the bottom of the game area. Swaps the player positions.
        /// </summary>
        [Server]
        public static void ResetBall()
        {
            _instance.SwapPlayers();
        }

        /// <summary>
        /// Swaps the positions of the players.
        /// </summary>
        [Server]
        public void SwapPlayers()
        {
            topPlayer = (topPlayer + 1) % players.Count;
            TopPlayer()?.TargetMoveTop();
            BottomPlayer()?.TargetMoveBottom();
        }

        /// <summary>
        /// Returns the top player.
        /// </summary>
        [Server]
        public static Paddle TopPlayer()
        {
            return _instance.players[_instance.topPlayer];
        }

        /// <summary>
        /// Returns the bottom player.
        /// </summary>
        [Server]
        public static Paddle BottomPlayer()
        {
            return _instance.players[(_instance.topPlayer + 1) % _instance.players.Count];
        }

        /// <summary>
        /// Sets the reference to the ball.
        /// </summary>
        [Server]
        public static void SetBall(Ball newBall)
        {
            _instance.ball = newBall;
        }
        
        /// <summary>
        /// Called by the <see cref="Paddle.CmdFire"/> command. Fires the ball.
        /// </summary>
        [Server]
        public static void FireBall()
        {
            if (_instance.ball) _instance.ball.Fired();
        }

        /// <summary>
        /// Called when the <see cref="Ball"/> collides. Shakes the game camera.
        /// </summary>
        public static void ShakeCam()
        {
            _instance.RpcShakeCam();
        }

        /// <summary>
        /// Called by <see cref="ShakeCam"/> to invoke the camera shake on all clients.
        /// </summary>
        [ClientRpc]
        public void RpcShakeCam()
        {
            CameraController.Shake();
        }
    }
}