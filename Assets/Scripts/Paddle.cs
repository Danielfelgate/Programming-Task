using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 xMinMax;
    [SerializeField] private Ball ball;

    [SerializeField] private int player;

    public bool serving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float move = 0;

        if ((player == 1 && Input.GetKey(KeyCode.A)) || (player == 2 && Input.GetKey(KeyCode.LeftArrow)))
        {
            move -= Time.deltaTime * speed;
        }
        if ((player == 1 && Input.GetKey(KeyCode.D)) || (player == 2 && Input.GetKey(KeyCode.RightArrow)))
        {
            move += Time.deltaTime * speed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }    

        float xPos = Mathf.Clamp(transform.localPosition.x + move, xMinMax.x, xMinMax.y);
        transform.localPosition = new Vector3(xPos, transform.localPosition.y, transform.localPosition.z);
    }

    private void LateUpdate()
    {
        if (serving)
        {
            ball.transform.position = transform.position + Vector3.up * 0.4f;
        }
    }

    void Fire()
    {
        if (serving)
        {
            serving = false;
            ball.Fired();
        }
    }

    public void MoveTop()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
    }

    public void MoveBottom()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, -1, transform.localPosition.z);
    }
}
