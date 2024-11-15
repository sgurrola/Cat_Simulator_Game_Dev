using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    // private List<KeyCode> keysPressed = new List<KeyCode>();
    // public InputAction playerControls;
    private Rigidbody2D rb;
    private Collider2D cl;
    private float horizontal;
    private float vertical;
    private bool isOnCurtain = false;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isFacingRight = true;
    private Vector3 spawnLoc;

    public float fallMult = 2.5f;
    public float lowJumpMult = 2f;
    
    private int score = 0;

    public ScoreManager scoreManager;
    public bool playerCaught;
    //private int score = 0;
    public LayerMask humanFieldLayer;

    public LayerMask safeZoneLayer;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Hello world! " + speed);
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponent<Collider2D>();
        spawnLoc = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFacingRight && horizontal < 0f) Flip();
        if (!isFacingRight && horizontal > 0f) Flip();
        
        if(Physics2D.OverlapCircle(this.transform.position, .1f, humanFieldLayer)) { //add result
            if(!Physics2D.OverlapCircle(this.transform.position, .1f, safeZoneLayer))
            {
                Debug.Log("You've been spotted!");
                playerCaught = true;
            }
            else {
                playerCaught = false;
                Debug.Log("Good Hiding");
            }
            
        }
        if (playerCaught) {
            if (scoreManager != null)
            {
                this.gameObject.SetActive(false);
                scoreManager.PlayerDied();
                MomentumStop();
                playerCaught = false;

            }
        }

    }

    void FixedUpdate() {
        if(isOnCurtain) {
            rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        }
        else if(rb.velocity.y < 0) { //coming down
            // Debug.Log("negative y vel");
            // rb.velocity = new Vector2(horizontal * .95f, rb.velocity.y);        //limited in air movement - creates issue with moving on top of pushable obj
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);    //unlimited in air movement going down
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump")) { //holding jump
            // Debug.Log("lowjumpmult used");
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMult - 1) * Time.deltaTime;
        }
        else rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

    }

    // void OnJump(InputValue value) {
    //     Debug.Log("Jumping");
    //     if (value.isPressed && IsGrounded()) {
    //         Debug.Log("Jumping real");
    //         rb.velocity = new Vector2(rb.velocity.x,jumpPower);
    //     }
    //     //else if(rb.velocity.y > 0f) rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y * .5f);
    // }

    void OnJump(InputValue value) {
        // Debug.Log("Jumping");
        if (value.isPressed && IsGrounded()) {
            // Debug.Log("Jumping real");
            rb.velocity = new Vector2(rb.velocity.x,jumpPower);
        }
        // else if(rb.velocity.y > 0f) rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y * .5f);
    }

    void OnMove(InputValue value) {  
        // Debug.Log("Moving: " + value.Get<float>());
        horizontal = value.Get<float>();             
    }

    void OnClimb(InputValue value) {
        // Debug.Log("Climbing");
        if(isOnCurtain) {
            Debug.Log("Climbing real");
            vertical = value.Get<float>();
        }
        if(!isOnCurtain) {                      //prevents weird curtain bug that forces player to only go up on curtains
            vertical = 0;
        }
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle( groundCheck.position, .5f, groundLayer);
    }

    private void Flip() {
        isFacingRight = !isFacingRight;
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == 11) {
            isOnCurtain = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == 11) {
            isOnCurtain = false;
        }
    }

    void OnReset(InputValue value) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
    public void MomentumStop() {
        horizontal = 0;
        vertical = 0;
    }
}
