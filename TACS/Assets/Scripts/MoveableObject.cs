using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    private SpriteRenderer sprite_renderer;
    private Rigidbody2D rigid_body;
    public ScoreManager scoreManager;
    private Vector2 startpos;
    private Quaternion startRot;
    public LayerMask humanFieldLayer;

    private void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        rigid_body = GetComponent<Rigidbody2D>();
        startpos = this.gameObject.transform.position;
        startRot = this.gameObject.transform.rotation;
    }
    // Start is called before the first frame update

    void Update ()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Destroyer") 
        {
            if (scoreManager != null)
            {
                if(this.gameObject.tag == "Moveable") 
                {
                    scoreManager.IncreaseScore(1);
                    Debug.Log("Score increased, current score: " + scoreManager.score);
                } else //else if (this.gameObject.tag == "Bomb")
                {
                    // Debug.Log("explosion should trigger");
                    // Debug.Log(GetComponentInChildren<ParticleSystem>());
                    GetComponentInChildren<ParticleSystem>().Play();
                    scoreManager.PlayerDied();
                }
                
            }
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<PolygonCollider2D>().enabled = false;
            Destroy(this.gameObject, 2f);
        }         
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("Triggered");
        if(other.gameObject.CompareTag("VisionField")) {
            // Debug.Log("Triggered on visionfield");

            //MOVES ALL OBJECTS COMPLETELY BACK TO START POS
            // this.gameObject.transform.position = startpos;
            InvokeRepeating("moveBack",0,.01f);

            // MOVES SOME OBJECTS COMPLETELY BACK TO START POS 
            // if(Random.Range(1,101) < 50) {
            //     this.gameObject.transform.position = startpos;
            // }
            // Debug.Log("block moved back to: "+startpos);
        }
    }

    private void moveBack() {
        this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        this.gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation,startRot,3f);
        this.gameObject.transform.position = Vector2.MoveTowards(transform.position,startpos,.095f);
        if(this.gameObject.transform.position.x == startpos.x && transform.position.y == startpos.y && transform.rotation == startRot) {
            this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
            CancelInvoke();
        }
    }
}