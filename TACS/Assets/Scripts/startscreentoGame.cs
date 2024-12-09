using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class startscreentoGame : MonoBehaviour
{
     public void GoToMainMenu() { 
        //SceneManager.LoadScene("level");
        Debug.Log("scene change 1");
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log("scene change 2");
        SceneManager.LoadScene(nextSceneIndex);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
