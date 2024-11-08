using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;         // The player's score
    public int maxscore = 4;
    public int lives = 9;
    public int maxlives = 9;
    public Text scoreText;        // Reference to the UI text element
    public Text lifeText;
    public float respawn_time = 3.0f;
    public GameObject player;

    // This function increases the score
    public void IncreaseScore(int amount)
    {
        score += amount;
        if (score == maxscore) {
            //advance to next level/scene
            UpdateScoreUI();
        } else {
            UpdateScoreUI();
        }
    }

    // Update the score display
    void UpdateScoreUI()
    {
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
            lifeText.text = "Lives: " + lives.ToString() + "/" + maxlives.ToString();
            Debug.Log("Health decreased, current health: " + lives + "/" + maxlives);
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
        player.GetComponent<PlayerMovement>().MomentumStop();
        player.gameObject.SetActive(true);
    }
}
