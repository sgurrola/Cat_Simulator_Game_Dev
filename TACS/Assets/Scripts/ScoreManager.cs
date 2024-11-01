using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;         // The player's score
    public int lives = 9;
    public Text scoreText;        // Reference to the UI text element
    public Text lifeText;
    public float respawn_time = 3.0f;
    public GameObject player;

    // This function increases the score
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    // Update the score display
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // Update the score display
    void UpdateLifeUI()
    {
        if (lifeText != null)
        {
            lifeText.text = "Lives: " + lives.ToString();
            Debug.Log("Health decreased, current health: " + lives);
        }
    }

    public void PlayerDied() 
    {
        player.gameObject.SetActive(false);
        this.lives--;
        UpdateLifeUI();

        if (this.lives <= 0) {
            //GameOver();
        } else {
            Invoke(nameof(Respawn), this.respawn_time);
        }

    }

    private void Respawn()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
    }
}
