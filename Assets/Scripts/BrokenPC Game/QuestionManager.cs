using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Question
{
    public string questionText;
    public int correctAnswerIndex;
    public string positiveFeedback;
    public string negativeFeedback;
}

public class QuestionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text questionText;
    public Button[] answerButtons;

    [Header("Questions")]
    public Question[] questions;

    private int currentQuestionIndex = 0;
    private bool isShowingFeedback = false;
    private int correctAnswersCount = 0;

    public VectorValue startingPositionDynamic;
    public VectorValue startingPositionPreviousScene;
    private PlayerMovement playerMovementScript;
    private bool isShowingFinalScore = false;

    public void StartGame()
    {
        LoadQuestion();
    }

    void LoadQuestion()
    {
        if (currentQuestionIndex >= 0 && currentQuestionIndex < questions.Length)
        {
            Question currentQuestion = questions[currentQuestionIndex];
            questionText.text = currentQuestion.questionText;

            for (int i = 0; i < answerButtons.Length; i++)
            {
                int index = i; // Copy of i to avaid closure
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
            }

            ToggleAnswerButtons(true);
        }
    }

    void CheckAnswer(int selectedIndex)
    {
        Question currentQuestion = questions[currentQuestionIndex];

        if (selectedIndex == currentQuestion.correctAnswerIndex)
        {
            Debug.Log("Risposta corretta!");
            questionText.text = currentQuestion.positiveFeedback;
            correctAnswersCount++;
        }
        else
        {
            Debug.Log("Risposta errata!");
            questionText.text = currentQuestion.negativeFeedback;
        }

        isShowingFeedback = true;

        ToggleAnswerButtons(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isShowingFeedback)
            {
                AdvanceToNextQuestion();
            }
            else if (isShowingFinalScore)
            {
                LoadNextScene();
            }
        }
    }

    void AdvanceToNextQuestion()
    {
        isShowingFeedback = false;

        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Length)
        {
            LoadQuestion();
        }
        else
        {
            Debug.Log("Fine del quiz!");

            ShowFinalScore();
            ToggleAnswerButtons(false);

            FindObjectOfType<IntroSequence>().CompleteQuiz();
        }
    }

    void ShowFinalScore()
    {
        isShowingFeedback = false;
        isShowingFinalScore = true;

        string message;

        if (correctAnswersCount >= 8)
        {
            message = $"Complimenti! Hai totalizzato {correctAnswersCount}/{questions.Length} domande corrette! Hai sbloccato un nuovo aspetto! Premi Spazio per ritornare in classe!";

            // Unlock a new skin
            if (SkinManager.Instance != null)
            {
                SkinManager.Instance.UnlockSkin(6);
                Debug.Log($"New skin unlocked: {SkinManager.Instance.skinNames[6]}");
            }

        }
        else if (correctAnswersCount >= questions.Length / 2)
        {
            message = $"Complimenti! Hai totalizzato {correctAnswersCount}/{questions.Length} domande corrette! Premi Spazio per ritornare in classe!";
        }
        else
        {
            message = $"Purtroppo non hai superato il test. Hai risposto correttamente a {correctAnswersCount}/{questions.Length} domande. Ritenta una prossima volta! Premi Spazio per ritornare in classe!";
        }

        questionText.text = message;
    }

    void LoadNextScene()
    {
        // Set player's position to the saved one so that PlayerMovements correctly position the player in the class scene
        startingPositionDynamic.initialValue = startingPositionPreviousScene.initialValue;
        SceneManager.LoadScene("LeftClassInterior");
    }

    void ToggleAnswerButtons(bool state)
    {
        foreach (Button button in answerButtons)
        {
            button.interactable = state;
        }
    }
}
