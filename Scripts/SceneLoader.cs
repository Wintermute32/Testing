using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//physics are fucking up. not calculating physics in time to match ballspeed.

public class SceneLoader : MonoBehaviour
{
    
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);

    }
   public void LoadStartScene()
    {
     SceneManager.LoadScene(0);
     FindObjectOfType<GameSession>().ResetGameScore(); 
    }

    public void LoadQuitScene()
    {
        Application.Quit();
    }
}
