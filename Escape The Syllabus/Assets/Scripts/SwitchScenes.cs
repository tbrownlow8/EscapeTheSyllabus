using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    public void ChangeScenes(string scenename)
    {
        // Shows button click in console
        Debug.Log("Clicked");

        // Loads scene with specific name
        SceneManager.LoadScene(scenename);
    }
}