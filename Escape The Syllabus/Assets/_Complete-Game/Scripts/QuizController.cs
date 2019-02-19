using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class QuizController : MonoBehaviour
{
    public Text questionText;
    public SimpleObjectPool answerButtonObjectPool;
    public Transform answerButtonParent;
    public Text scoreDisplayText;
    public GameObject questionDisplay;
    public GameObject roundEndDisplay;

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();

    private bool isRoundActive;
    private float timeRemaining;
    private int questionIndex;
    private int playerScore;

    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.getCurrentRoundData();
        questionPool = currentRoundData.questions;
        timeRemaining = currentRoundData.timeLimitInSeconds;

        playerScore = 0;
        questionIndex = 0;

        ShowQuestion();
        isRoundActive = true;
        roundEndDisplay.SetActive(false);

    }

    private void ShowQuestion()
    {
        RemoveAnswerButtons();
        QuestionData questionData = questionPool[questionIndex];
        questionText.text = questionData.questionText;

        for (int i = 0; i < questionData.answers.Length; i++)
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            answerButtonGameObject.transform.SetParent(answerButtonParent);
            answerButtonGameObjects.Add(answerButtonGameObject);

            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.setUp(questionData.answers[i]);
        }
    }

    private void RemoveAnswerButtons()
    {
        while(answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }

    public void AnswerButtonClicked(bool isCorrect)
    {
        if (isCorrect)
        {
            playerScore += currentRoundData.pointsScored;
            scoreDisplayText.text = "Score: " + playerScore.ToString();
        }
        if (questionPool.Length > questionIndex + 1)
        {
            questionIndex++;
            ShowQuestion();
        } else
        {
            EndRound();
        }
    }

    public void EndRound()
    {
        isRoundActive = false;
        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
