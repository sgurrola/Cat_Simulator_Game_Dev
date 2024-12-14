using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager Instance { get; private set; }

    // Example variables that can be accessed and modified by other scripts
    public int score;
    public int lives;
    public int maxlives;
    public int catType;
    public int levelnum;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Make persistent across scenes
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.name == "startScreen")
        {
            lives = maxlives;
            levelnum = 1;
        }
    }
}
