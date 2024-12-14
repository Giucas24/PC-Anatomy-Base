using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex;
    }

    [System.Serializable]
    public class AnswerSummary
    {
        public string questionText;
        public string userAnswer;
        public string correctAnswer;
    }

    public PlayerMovement playerMovement;
    private List<AnswerSummary> answerSummaries = new List<AnswerSummary>();

    public List<Question> questions;
    private int currentQuestionIndex = 0;
    private int correctAnswersCount = 0;
    private int incorrectAnswersCount = 0;

    public Text questionText;
    public Button[] answerButtons;
    public GameObject resultsPanel;
    public Text resultsText;

    public GameObject questionPanel;

    private SkinManager skinManager;

    void Start()
    {
        skinManager = FindObjectOfType<SkinManager>();

        resultsPanel.SetActive(false);
        questionPanel.SetActive(false);
    }

    public void StartQuiz()
    {
        currentQuestionIndex = 0;
        correctAnswersCount = 0;
        incorrectAnswersCount = 0;

        questionText.gameObject.SetActive(true);
        foreach (Button btn in answerButtons)
        {
            btn.gameObject.SetActive(true);
        }

        resultsPanel.SetActive(false);
        questionPanel.SetActive(true);
        LoadQuestion();
    }

    void LoadQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            questionText.text = questions[currentQuestionIndex].questionText;

            for (int i = 0; i < answerButtons.Length; i++)
            {
                Text answerText = answerButtons[i].GetComponentInChildren<Text>();
                if (answerText != null)
                {
                    answerText.text = questions[currentQuestionIndex].answers[i];
                }

                answerButtons[i].onClick.RemoveAllListeners();

                int answerIndex = i;
                answerButtons[i].onClick.AddListener(() => CheckAnswer(answerIndex));
            }
        }
        else
        {
            ShowResults();
        }
    }

    void CheckAnswer(int selectedAnswerIndex)
    {
        if (currentQuestionIndex >= questions.Count)
        {
            return;
        }

        var summary = new AnswerSummary
        {
            questionText = questions[currentQuestionIndex].questionText,
            userAnswer = questions[currentQuestionIndex].answers[selectedAnswerIndex],
            correctAnswer = questions[currentQuestionIndex].answers[questions[currentQuestionIndex].correctAnswerIndex]
        };
        answerSummaries.Add(summary);

        if (selectedAnswerIndex == questions[currentQuestionIndex].correctAnswerIndex)
        {
            correctAnswersCount++;
            Debug.Log("Correct Answer!");
        }
        else
        {
            incorrectAnswersCount++;
            Debug.Log("Incorrect Answer!");
        }

        currentQuestionIndex++;
        LoadQuestion();
    }

    void ShowResults()
    {
        questionPanel.SetActive(false);
        foreach (Button btn in answerButtons)
        {
            btn.gameObject.SetActive(false);
        }

        resultsPanel.SetActive(true);

        string resultMessage = "<b>Risultati del Quiz</b>\n\n";
        foreach (var summary in answerSummaries)
        {
            resultMessage += "<b>Domanda:</b> " + summary.questionText + "\n";
            resultMessage += "<i>Risposta dell'utente:</i> " + summary.userAnswer + "\n";
            resultMessage += "<i>Risposta corretta:</i> " + summary.correctAnswer + "\n\n";
        }

        resultMessage += "<color=green>Risposte corrette: " + correctAnswersCount + "</color>\n";
        resultMessage += "<color=red>Risposte errate: " + incorrectAnswersCount + "</color>\n\n";

        if (correctAnswersCount >= 8)
        {
            // Unlock a new skin
            if (SkinManager.Instance != null)
            {
                SkinManager.Instance.UnlockSkin(3);
                Debug.Log($"New skin unlocked: {SkinManager.Instance.skinNames[3]}");
                resultMessage += "<color=Green><b>Congratulazioni! Hai sbloccato un nuovo aspetto!</b></color>\n\n";
            }

        }

        resultMessage += "<i>Premi Invio per chiudere questo pannello e continuare il gioco.</i>";

        resultsText.text = resultMessage;
    }


    public void RestartQuiz()
    {
        currentQuestionIndex = 0;
        correctAnswersCount = 0;
        incorrectAnswersCount = 0;
        answerSummaries.Clear();

        questionPanel.SetActive(true);
        resultsPanel.SetActive(false);

        LoadQuestion();
    }

    void Update()
    {
        if (resultsPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            resultsPanel.SetActive(false);

            questionPanel.SetActive(false);
            foreach (Button btn in answerButtons)
            {
                btn.gameObject.SetActive(false);
            }

            if (playerMovement != null)
            {
                playerMovement.isQuizActive = false;
            }
        }
    }
}
