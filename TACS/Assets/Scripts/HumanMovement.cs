using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HumanMovement : MonoBehaviour
{
    public float speed = 0;
    public LayerMask humanFieldLayer;
    public GameObject humanField;
    public GameObject player;
    public bool playerCaught;
    public GameObject humanReset;
    public LayerMask safeZoneLayer;

    bool hasRotated = false;

    //public float hTimer = 15f;

    void Start()
    {
        InvokeRepeating("HumanStart", 15.0f, 12f); //swap for coroutine
        //https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html
    }
    void Update(){
        if(Physics2D.OverlapCircle(player.transform.position, .1f, humanFieldLayer)) { //add result
            if(!Physics2D.OverlapCircle(player.transform.position, .1f, safeZoneLayer))
            {
                Debug.Log("You've been spotted!");
            //scene reset
            playerCaught = true;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            Debug.Log("Good Hiding");
        }
        else {
            playerCaught = false;
        }

        transform.Translate(Vector3.right * speed * Time.deltaTime);
        //humanField.transform.Translate(Vector3.right * speed * Time.deltaTime);

        if(Physics2D.OverlapCircle(humanReset.transform.position, .1f, humanFieldLayer))
        {
            transform.position = new Vector3(-23.5f, -2f, 0f);
            humanField.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            hasRotated = false;

            
            speed = 0;
        }
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Moveable");
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (GameObject box in boxes)
        {
            foreach(GameObject bomb in bombs) {
                Rigidbody2D boxRigidbody = box.GetComponent<Rigidbody2D>();

            // Check if the box has a Rigidbody2D and its speed is greater than 0.1
                if (boxRigidbody != null && boxRigidbody.velocity.magnitude > 1f)
                {
                if (Physics2D.OverlapCircle(box.transform.position, 0.1f, humanFieldLayer) && !hasRotated)
                {
                    // Rotate the humanField object
                    humanField.transform.Rotate(0f, 0f, .02f, Space.Self);
                    Debug.Log("Box has moved!");
                    hasRotated = true;
                }
                }
            }
        }
    }

    void HumanStart() {
        speed = 5;
        }
}