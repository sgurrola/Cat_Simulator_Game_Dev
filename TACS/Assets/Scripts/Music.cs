using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    private static Music instance;
    private AudioSource audioSource;

    public AudioClip defaultMusic; // Assign the default music clip in the Inspector
    public AudioClip gameOverMusic; // Assign a specific clip for another scene
    public AudioClip victoryrMusic; // Assign a specific clip for another scene

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes

        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        audioSource.clip = defaultMusic; // Start with default music
        audioSource.Play();
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
        // Switch music based on scene name or index
        if (scene.name == "gameOver")
        {
            ChangeMusic(gameOverMusic);
        }
        else if (scene.name == "gameWin")
        {
            ChangeMusic(victoryrMusic);
        }
        else if (scene.name == "SilentScene")
        {
            StopMusic(); // Turn off music
        }
        else
        {
            ChangeMusic(defaultMusic);
        }
    }

    public void ChangeMusic(AudioClip newClip)
    {
        if (audioSource.clip == newClip) return; // Avoid restarting the same music

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
