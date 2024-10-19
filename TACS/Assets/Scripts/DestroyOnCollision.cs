using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    // Reference to the score manager (or you can handle the score here directly)
    public ScoreManager scoreManager;
    // This function is called when the object collides with another
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Moveable object collided with destroyer");
        // Check if the object colliding has the tag "Moveable"
        if (collision.gameObject.CompareTag("Moveable"))
        {
            Debug.Log("Destroyer tag matched, destroying moveable object");
            // Check if this object has the tag "Destroyer"
            if (gameObject.CompareTag("Destroyer"))
            {
                // Destroy the moveable object
                Destroy(collision.gameObject);

                // Increase the score
                if (scoreManager != null)
                {
                    scoreManager.IncreaseScore(1);  // Increase the score by 1
                }
            }
        }
    }
}
