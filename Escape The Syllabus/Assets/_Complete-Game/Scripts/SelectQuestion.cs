using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectQuestion : MonoBehaviour
{
    public GameObject Question1;
    public GameObject Question2;
    public GameObject Question3;
    public GameObject Question4;
    public GameObject Question5;
    public GameObject Question6;
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
        if (QuestionStats.questionNumber == 3)
        {
            Question2.SetActive(false);
            Question3.SetActive(true);
        }
        if (QuestionStats.questionNumber == 4)
        {
            Question3.SetActive(false);
            Question4.SetActive(true);
        }
        if (QuestionStats.questionNumber == 5)
        {
            Question4.SetActive(false);
            Question5.SetActive(true);
        }
        if (QuestionStats.questionNumber == 6)
        {
            Question5.SetActive(false);
            Question6.SetActive(true);
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
