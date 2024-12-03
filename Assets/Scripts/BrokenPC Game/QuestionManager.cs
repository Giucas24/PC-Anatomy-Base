using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

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

    private int currentQuestionIndex = 0;  // Indice della domanda corrente
    private bool isShowingFeedback = false; // Indica se stiamo mostrando un feedback
    private int correctAnswersCount = 0; // Conteggio delle risposte corrette

    public void StartGame()
    {
        LoadQuestion(); // Carica la prima domanda
    }

    void LoadQuestion()
    {
        // Log di debug per monitorare l'indice
        Debug.Log("SIAMO DENTRO LOADQUESTION, MOSTRO currentQuestionindex: " + currentQuestionIndex);

        // Controlla che l'indice sia valido
        if (currentQuestionIndex >= 0 && currentQuestionIndex < questions.Length)
        {
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
            correctAnswersCount++; // Incrementa il conteggio delle risposte corrette
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
            ShowFinalScore(); // Mostra il punteggio finale
            ToggleAnswerButtons(false); // Disabilita i bottoni

            // Chiama il metodo per segnare il completamento del quiz
            FindObjectOfType<IntroSequence>().CompleteQuiz();
        }
    }

    void ShowFinalScore()
    {
        // Determina il messaggio da mostrare in base al punteggio
        string message;

        if (correctAnswersCount >= 8) // Se il giocatore ha risposto correttamente ad almeno 8 domande
        {
            message = $"Complimenti! Hai totalizzato {correctAnswersCount}/{questions.Length} domande corrette! Hai sbloccato un nuovo aspetto!";

            // Sblocca il nuovo aspetto (skin)
            if (SkinManager.Instance != null)
            {
                Debug.Log("Chiamata a SkinManager.Instance.UnlockSkin()");
                SkinManager.Instance.UnlockSkin(4); // Indice 5 per sbloccare una skin
            }
            else
            {
                Debug.LogWarning("SkinManager.Instance è null!");
            }
        }
        else if (correctAnswersCount >= questions.Length / 2) // Test superato
        {
            message = $"Complimenti! Hai totalizzato {correctAnswersCount}/{questions.Length} domande corrette!";
        }
        else // Test non superato
        {
            message = $"Purtroppo non hai superato il test. Hai risposto correttamente a {correctAnswersCount}/{questions.Length} domande. Ritenta una prossima volta!";
        }

        // Mostra il messaggio finale
        questionText.text = message;

        // Controlla se il giocatore preme Spazio per avanzare
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Carica la scena della classe
            SceneManager.LoadScene("LeftClassInterior");
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
