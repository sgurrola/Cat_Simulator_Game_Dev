using UnityEngine;
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
    public AudioClip soundA; // Assign the sound for Object A
    public AudioClip soundB; // Assign the sound for Object B

    private void Awake()
    {
        lifeText.text = "Lives: " + PersistentManager.Instance.lives.ToString() + "/" + PersistentManager.Instance.maxlives.ToString();
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
    // This function increases the score
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    // Update the score display
    void UpdateScoreUI()
    {   
        if (score >= maxscore) {
            //advance to next level/scene
            Debug.Log("scene change 1");
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            Debug.Log("scene change 2");
            SceneManager.LoadScene(nextSceneIndex);
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString() + "/" + maxscore.ToString();
        }
    }

    // Update the score display
    void UpdateLifeUI()
    {
        if (lifeText != null)
        {
            lifeText.text = "Lives: " + PersistentManager.Instance.lives.ToString() + "/" + PersistentManager.Instance.maxlives.ToString();
            Debug.Log("Health decreased, current health: " + PersistentManager.Instance.lives + "/" + PersistentManager.Instance.maxlives);
        }
    }

    public void PlayerWon() {
        if (score == 4) {
            SceneManager.LoadScene("gameWin");
        }
    }

    public void PlayerDied() 
    {
        player.gameObject.SetActive(false);
        PersistentManager.Instance.lives -= 1;
        //this.lives--;
        UpdateLifeUI();

        if (PersistentManager.Instance.lives <= 0) {
            //GameOver();
            SceneManager.LoadScene("gameOver");
        } else {
            InvokeRepeating("Respawn",1,1);
        }

    }

    private void Respawn()
    {
        if(!human.GetComponent<HumanMovement>().isMoving) {
            player.transform.position = Vector3.zero;
            player.GetComponent<PlayerMovement>().MomentumStop();
            player.gameObject.SetActive(true);
            CancelInvoke();
        }
    }
}
