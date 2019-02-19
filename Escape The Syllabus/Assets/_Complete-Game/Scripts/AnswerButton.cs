using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    public Text answerText;
    private QuizController quizController;
    private AnswerData answerData;
    // Start is called before the first frame update
    void Start()
    {
        quizController = FindObjectOfType<QuizController>();
    }

    public void setUp(AnswerData data)
    {
        answerData = data;
        answerText.text = answerData.answerText;
    }

    public void HandleClick()
    {
        quizController.AnswerButtonClicked(answerData.isCorrect);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
