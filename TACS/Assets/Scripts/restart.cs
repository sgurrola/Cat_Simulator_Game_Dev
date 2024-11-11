using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartLevel : MonoBehaviour
{
    void Start()
    {
        Button restartButton = GetComponent<Button>();
        restartButton.onClick.AddListener(RestartCurrentLevel);
    }

    void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
