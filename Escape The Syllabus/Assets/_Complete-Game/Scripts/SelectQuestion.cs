using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectQuestion : MonoBehaviour
{
    public GameObject Question1;
    public GameObject Question2;
    // Start is called before the first frame update
    void Start()
    {
        if (QuestionStats.questionNumber == 1)
        {
            Question1.SetActive(true);
        }
        if (QuestionStats.questionNumber == 2)
        {
            Question1.SetActive(false);
            Question2.SetActive(true);
        }
        
    }
    public void updateCorrect(bool ans)
    {
        QuestionStats.wasCorrect = ans;
    }
    public void updatePlayer()
    {
        PlayerLocation.updatePlayer = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
