using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    public ScoreManager scoreManager;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Moveable"))
        {
            Debug.Log("Moveable object collided with destroyer");

            if (gameObject.CompareTag("Destroyer"))
            {
                Debug.Log("Destroyer tag matched, deactivating moveable object");

                // Deactivate the moveable object instead of destroying it
                collision.gameObject.SetActive(false);

                // Increase the score
                if (scoreManager != null)
                {
                    scoreManager.IncreaseScore(1);
                    Debug.Log("Score increased, current score: " + scoreManager.score);
                }
            }
        }
    }
}
