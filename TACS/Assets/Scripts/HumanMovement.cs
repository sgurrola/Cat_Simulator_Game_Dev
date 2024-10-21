using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanMovement : MonoBehaviour
{
    public float speed = 0;
    public LayerMask humanFieldLayer;
    public GameObject humanField;
    public GameObject player;
    public bool playerCaught;
    public GameObject humanReset;
    public bool humanStop;
    void Update(){
        if (Input.GetKey("h"))
        {
            speed = 5;
        }
        //is cat in human sight
        if(Physics2D.OverlapCircle(player.transform.position, .1f, humanFieldLayer)) {
            Debug.Log("You've been spotted!");
            playerCaught = true;
        }
        else {
            playerCaught = false;
        }

        transform.Translate(Vector3.right * speed * Time.deltaTime);
        humanField.transform.Translate(Vector3.right * speed * Time.deltaTime);

        if(Physics2D.OverlapCircle(humanReset.transform.position, .1f, humanFieldLayer))
        {
            transform.position = new Vector3(-23.5f, -2f, 0f);
            humanField.transform.position = new Vector3(-18f, -2f, 0f);
            speed = 0;
        }
    }

}