using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > Mathf.Epsilon)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CameraController.Shake();

        if (collision.gameObject.CompareTag("Brick"))
        {
            Brick brick = collision.gameObject.GetComponent<Brick>();
            if (brick) brick.Break();
        }
        else if (collision.gameObject.CompareTag("Destroyer"))
        {
            rb.simulated = false;
            GameManager.ResetBall();
        }
    }

    public void Fired()
    {
        rb.simulated = true;
        rb.velocity = new Vector2(Random.value * 2 - 1, 1);
    }
}
