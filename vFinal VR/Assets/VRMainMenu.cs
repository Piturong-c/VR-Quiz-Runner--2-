using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRMainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    public void SpeedMode()
    {
        SceneManager.LoadScene("ZenModeScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
