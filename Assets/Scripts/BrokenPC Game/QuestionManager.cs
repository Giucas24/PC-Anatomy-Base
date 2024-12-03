using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Question
{
    public string questionText; // Testo della domanda
    public int correctAnswerIndex; // Indice del bottone corretto
    public string positiveFeedback; // Feedback per risposta corretta
    public string negativeFeedback; // Feedback per risposta sbagliata
}

public class QuestionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text questionText; // Riferimento al TextMeshPro per la domanda
    public Button[] answerButtons; // Bottoni che rappresentano le componenti hardware (CPU, GPU, ecc.)

    [Header("Questions")]
    public Question[] questions; // Domande configurabili dall'Inspector

    private int currentQuestionIndex; // Indice della domanda corrente
    private bool isShowingFeedback = false; // Indica se stiamo mostrando un feedback


    public void StartGame()
    {
        currentQuestionIndex = 0; // Inizia dalla prima domanda
        LoadQuestion();
    }

    void LoadQuestion()
    {
        // Controlla che l'indice sia valido
        if (currentQuestionIndex >= 0 && currentQuestionIndex < questions.Length)
        {
            Debug.Log("SIAMO DENTRO LOADQUESTION, MOSTRO currentQuestionindex: " + currentQuestionIndex);
            // Ottieni la nuova domanda
            Question currentQuestion = questions[currentQuestionIndex];
            questionText.text = currentQuestion.questionText;

            // Riassegna i listener ai bottoni
            for (int i = 0; i < answerButtons.Length; i++)
            {
                int index = i; // Variabile locale per evitare problemi con il closure
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
            }

            // Riabilita i bottoni
            ToggleAnswerButtons(true);
        }
    }

    void CheckAnswer(int selectedIndex)
    {
        // Ottieni la domanda corrente
        Question currentQuestion = questions[currentQuestionIndex];

        // Mostra il feedback appropriato
        if (selectedIndex == currentQuestion.correctAnswerIndex)
        {
            Debug.Log("Risposta corretta!");
            questionText.text = currentQuestion.positiveFeedback;
        }
        else
        {
            Debug.Log("Risposta errata!");
            questionText.text = currentQuestion.negativeFeedback;
        }

        // Passa alla fase di feedback
        isShowingFeedback = true;

        // Disabilita temporaneamente i bottoni durante il feedback
        ToggleAnswerButtons(false);
    }


    void Update()
    {
        // Controlla se il giocatore preme Spazio per avanzare
        if (Input.GetKeyDown(KeyCode.Space) && isShowingFeedback)
        {
            AdvanceToNextQuestion(); // Passa alla domanda successiva
        }
    }

    void AdvanceToNextQuestion()
    {
        // Esci dalla fase di feedback
        isShowingFeedback = false;

        // Incrementa l'indice della domanda
        currentQuestionIndex++;

        if (currentQuestionIndex < questions.Length)
        {
            LoadQuestion(); // Carica la domanda successiva
        }
        else
        {
            Debug.Log("Fine del quiz!");
            // Logica per la fine del gioco (es. mostra punteggio o messaggio finale)
            questionText.text = "Quiz completato!"; // Messaggio finale
            ToggleAnswerButtons(false); // Disabilita i bottoni
        }
    }

    void ToggleAnswerButtons(bool state)
    {
        // Abilita o disabilita i bottoni
        foreach (Button button in answerButtons)
        {
            button.interactable = state;
        }
    }
}
