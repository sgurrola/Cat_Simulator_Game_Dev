using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;         // The player's score
    public Text scoreText;        // Reference to the UI text element

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
}
