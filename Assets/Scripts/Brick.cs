using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Brick : MonoBehaviour
{
    public Collider2D col;
    public SpriteShapeRenderer rend;
    public ParticleSystem part;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break()
    {
        col.enabled = false;
        rend.enabled = false;

        ParticleSystem.MainModule main = part.main;
        main.startColor = rend.color;
        part.Play();
        Destroy(gameObject, 1);
    }
}
