using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HumanMovement : MonoBehaviour
{
    public float speed = 4;
    public LayerMask humanFieldLayer;
    public GameObject humanField;
    public GameObject player;
    public bool playerCaught;
    public GameObject humanReset;
    public LayerMask safeZoneLayer;

    bool hasRotated = false;

    public bool isMoving = false;

    //public float hTimer = 15f;
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip stepSound; // Single step sound clip
    public float maxVolume = 1.0f; // Maximum walking volume
    public float minVolume = 0.2f; // Minimum walking volume
    public float pitchHigh = 1.0f; // Normal pitch
    public float pitchLow = 0.6f; // Slowed-down pitch
    public float stepInterval = 0.5f; // Time between steps
    private Coroutine walkingSoundCoroutine;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = stepSound;
        audioSource.volume = 0;
        audioSource.loop = false; // Manually loop the single step sound

        InvokeRepeating("HumanStart", 20.0f, 15.0f); //swap for coroutine
        //https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html
    }
    void Update(){

        //THIS HAS BEEN MOVED TO PLAYERMOVEMENT - MIGHT DELETE LATER
        // if(Physics2D.OverlapCircle(player.transform.position, .1f, humanFieldLayer)) { //add result
        //     if(!Physics2D.OverlapCircle(player.transform.position, .1f, safeZoneLayer))
        //     {
        //         Debug.Log("You've been spotted!");
        //         playerCaught = true;
        //     }
        //     else {
        //         playerCaught = false;
        //         Debug.Log("Good Hiding");
        //     }
            
        // }
        
        

        transform.Translate(Vector3.right * speed * Time.deltaTime);
        //humanField.transform.Translate(Vector3.right * speed * Time.deltaTime);

        if(Physics2D.OverlapCircle(humanReset.transform.position, .1f, humanFieldLayer))
        {
            transform.position = new Vector3(-23.5f, -2f, 0f);
            humanField.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            hasRotated = false;

            speed = 0;
            isMoving = false;  

            // Fade out the sound and lower pitch
            if (walkingSoundCoroutine != null) StopCoroutine(walkingSoundCoroutine);
            StartCoroutine(HandleResetFade()); 
        }
        else if (isMoving && walkingSoundCoroutine == null)
        {
            // Start looping walking sound
            walkingSoundCoroutine = StartCoroutine(PlayWalkingSound());
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
        isMoving = true;
        speed = 5;

        // Fade in the sound and reset pitch
        if (walkingSoundCoroutine != null) StopCoroutine(walkingSoundCoroutine);
        StartCoroutine(FadeInSound(pitchHigh));
    }

        // Coroutine to handle reset fade logic
    IEnumerator HandleResetFade()
    {
        // Lower the pitch and fade out the volume
        yield return StartCoroutine(FadeOutSound(pitchLow));

        // Immediately fade the sound back in after the reset
        yield return StartCoroutine(FadeInSound(pitchHigh));
        walkingSoundCoroutine = StartCoroutine(PlayWalkingSound());
    }

    // Coroutine to play walking sound with manual looping
    IEnumerator PlayWalkingSound()
    {
        while (isMoving)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); // Play the step sound
            }

            yield return new WaitForSeconds(stepInterval); // Wait before playing the next step
        }
    }

    // Coroutine to fade out sound and lower pitch
    IEnumerator FadeOutSound(float targetPitch)
    {
        float fadeDuration = 1.0f;
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, minVolume, t / fadeDuration);
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = minVolume;
        audioSource.pitch = targetPitch;
    }

    // Coroutine to fade in sound and raise pitch
    IEnumerator FadeInSound(float targetPitch)
    {
        float fadeDuration = 1.0f;
        float startVolume = audioSource.volume;

        audioSource.pitch = targetPitch;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, maxVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = maxVolume;
    }
}