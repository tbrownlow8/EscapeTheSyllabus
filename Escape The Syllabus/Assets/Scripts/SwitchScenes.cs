using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    public void ChangeScenes(string scenename)
    {
        // Shows button click in console
        Debug.Log("changed scenes");

        // Loads scene with specific name
        SceneManager.LoadScene(scenename);
        if (scenename == "EscapeTheSyllabusUI") {
          // Show score
          // FirebaseChecks.instance.GetComponentInChildren<Canvas>().enabled = false;
          // show ui screens
          GameObject.Find("Screens").GetComponent<Canvas>().enabled = true;
        } else {
          // Hide score
          // FirebaseChecks.instance.GetComponentInChildren<Canvas>().enabled = true;
          // load level
          SceneManager.LoadScene(scenename);
          // hide ui screens
          GameObject.Find("Screens").GetComponent<Canvas>().enabled = false;
        }
    }
}
