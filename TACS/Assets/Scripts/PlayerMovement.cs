using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public ScoreManager scoreManager;
    public bool playerCaught;
    //private int score = 0;
    public LayerMask humanFieldLayer;
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
        
        if(Physics2D.OverlapCircle(this.gameObject.transform.position, .1f, humanFieldLayer)) { //add result
            Debug.Log("You've been spotted! player!");
            //scene reset
            playerCaught = true;
        }
        else {
            playerCaught = false;
        }
        if (playerCaught) {
            if (scoreManager != null)
            {
                this.gameObject.SetActive(false);
                scoreManager.PlayerDied();
            }
        }

    }

    void FixedUpdate() {
        if(isOnCurtain) {
            rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        }
        else rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    void OnJump(InputValue value) {
        Debug.Log("Jumping");
        if (value.isPressed && IsGrounded()) {
            Debug.Log("Jumping real");
            rb.velocity = new Vector2(rb.velocity.x,jumpPower);
        }
        else if(rb.velocity.y > 0f) rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y * .5f);
    }

    void OnMove(InputValue value) {
        Debug.Log("Moving: " + value.Get<float>());
        horizontal = value.Get<float>();
    }

    void OnClimb(InputValue value) {
        Debug.Log("Climbing");
        if(isOnCurtain) {
            Debug.Log("Climbing real");
            vertical = value.Get<float>();
        }
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle( groundCheck.position, .1f, groundLayer);
    }

    private void Flip() {
        isFacingRight = !isFacingRight;
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == 8) {
            isOnCurtain = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == 8) {
            isOnCurtain = false;
        }
    }
}
