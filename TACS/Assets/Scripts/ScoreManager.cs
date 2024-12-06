using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public int score = 0;         // The player's score
    public int maxscore = 4;
    public Text scoreText;        // Reference to the UI text element
    public Text lifeText;
    public float respawn_time = 3.0f;
    public GameObject player;
    public GameObject human;
    public AudioSource audioSource; // Assign the Audio Source in the Inspector
    public AudioClip soundA; // Assign the sound for bottle break
    public AudioClip soundB; // Assign the sound for explosion
    public AudioClip soundC; // Assign the sound for level completion
    public AudioClip soundD; // Assign the sound for player death
    public AudioClip soundE; // Assign the sound for game failure
    public float delayDuration = 1.0f;  // Duration to wait before transitioning

    private void Awake()
    {
        UpdateLifeScoreUI();
    }

    public void PushableBroke(int amount)
    {
        audioSource.PlayOneShot(soundA);
        PersistentManager.Instance.score += 1;
        IncreaseScore(amount);
    }
    public void BombBroke()
    {
        audioSource.PlayOneShot(soundB);
        PersistentManager.Instance.score += 1;
        PlayerDied();
    }
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateLifeScoreUI();
    }
    public void PlayerDied() 
    {
        player.gameObject.SetActive(false);
        audioSource.PlayOneShot(soundD);
        PersistentManager.Instance.lives -= 1;
        UpdateLifeScoreUI();

    }
    void UpdateLifeScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString() + "/" + maxscore.ToString();
        }
        if (lifeText != null)
        {
            lifeText.text = "Lives: " + PersistentManager.Instance.lives.ToString() + "/" + PersistentManager.Instance.maxlives.ToString();
            Debug.Log("Health decreased, current health: " + PersistentManager.Instance.lives + "/" + PersistentManager.Instance.maxlives);
        }

        if (PersistentManager.Instance.lives <= 0) {
            //game over
            StartCoroutine(HandleFailure());
        } else if (!player.activeSelf) {
            InvokeRepeating("Respawn",1,1);
        }   

        if (score >= maxscore) {
            //advance to next level/scene
            StartCoroutine(HandleCompletion());
        }     
    }

    private void Respawn()
    {
        if(!human.GetComponent<HumanMovement>().isMoving) {
            player.transform.position = player.GetComponent<PlayerMovement>().spawnLoc;
            player.GetComponent<PlayerMovement>().MomentumStop();
            player.gameObject.SetActive(true);
            CancelInvoke();
        }
    }

    private IEnumerator HandleCompletion()
    {
        yield return new WaitForSeconds(delayDuration);

        // Play the completion sound
        if (audioSource != null && soundC != null)
        {
            audioSource.PlayOneShot(soundC);
        }
        
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        // Wait for the sound to finish or the specified delay duration
        yield return new WaitForSeconds(delayDuration);

        // Transition to the next scene
        SceneManager.LoadScene(nextSceneIndex);
    }

    private IEnumerator HandleFailure()
    {
        yield return new WaitForSeconds(delayDuration);

        // Play the completion sound
        if (audioSource != null && soundE != null)
        {
            audioSource.PlayOneShot(soundE); //switch to failure sound
        }

        // Wait for the sound to finish or the specified delay duration
        yield return new WaitForSeconds(delayDuration);

        // Transition to the game over scene
        SceneManager.LoadScene("gameOver");
    }
}
