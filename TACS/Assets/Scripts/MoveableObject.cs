using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    private SpriteRenderer sprite_renderer;
    private Rigidbody2D rigid_body;
    public ScoreManager scoreManager;
    private Vector2 startpos;
    public LayerMask humanFieldLayer;

    private void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        rigid_body = GetComponent<Rigidbody2D>();
        startpos = this.gameObject.transform.position;
    }
    // Start is called before the first frame update

    void Update ()
    {
        if(Physics2D.OverlapCircle(this.gameObject.transform.position, .1f, humanFieldLayer)) { //add result
            Debug.Log("You've been spotted! block!");
            this.gameObject.transform.position = startpos;
            Debug.Log("startposition"+startpos);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Destroyer") 
        {
            Destroy(this.gameObject);
            if (scoreManager != null)
            {
                if(this.gameObject.tag == "Moveable") 
                {
                    scoreManager.IncreaseScore(1);
                    Debug.Log("Score increased, current score: " + scoreManager.score);
                } else //else if (this.gameObject.tag == "Bomb")
                {
                    scoreManager.PlayerDied();
                }
                
            }
        }         
    }
}