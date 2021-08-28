using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private Paddle[] players;

    private int servingNext = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance) Destroy(_instance.gameObject);
        _instance = this;

        ResetBall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ResetBall()
    {
        _instance.players[_instance.servingNext].serving = true;
        _instance.players[_instance.servingNext].MoveTop();
        _instance.servingNext = (_instance.servingNext + 1) % _instance.players.Length;
        _instance.players[_instance.servingNext].MoveBottom();
    }
}
