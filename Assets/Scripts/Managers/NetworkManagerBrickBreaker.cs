using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrickBreaker
{
    /// <summary>
    /// Handles the connection and disconnection of players as well as spawning the ball and restarting the scene when all clients disconnect.
    /// </summary>
    public class NetworkManagerBrickBreaker : NetworkManager
    {
        /// <summary>Private reference to the instantiated ball.</summary>
        private GameObject ball;

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            // Spawn a player at the correct spawn position
            Paddle player = Instantiate(
                playerPrefab, 
                Vector3.up * (numPlayers == 0 ? Paddle.GetTopPaddleHeight() : Paddle.GetBottomPaddleHeight()), 
                Quaternion.identity).GetComponent<Paddle>();
            
            // Track the player in the GameManager
            GameManager.AddPlayer(player);

            NetworkServer.AddPlayerForConnection(conn, player.gameObject);

            // Spawn the ball if two players are connected
            if (numPlayers == 2)
            {
                ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
                NetworkServer.Spawn(ball);
                
                // Track the ball in the GameManager
                GameManager.SetBall(ball.GetComponent<Ball>());
            }
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            // Remove the player from the GameManager
            GameManager.RemovePlayer(conn.identity.GetComponent<Paddle>());

            // Destroy the ball
            if (ball != null) NetworkServer.Destroy(ball);

            base.OnServerDisconnect(conn);

            // If the game is running as a server build, stop the server when all client's disconnect to allow the game to reset
#if UNITY_SERVER
            if (autoStartServerBuild)
            {
                if (numPlayers == 0) StopServer();
            }
#endif
        }

        public override void OnStopServer()
        {
            base.OnStopServer();

            // Restarts the game when the server is stopped
            SceneManager.LoadScene(0);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);

            // Resets the score text when the client disconnects from the server
            GameManager.ResetScoreText();
        }
    }
}
