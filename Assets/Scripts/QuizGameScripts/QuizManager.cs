using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers; // Array di 4 risposte possibili
        public int correctAnswerIndex; // Indice della risposta corretta (da 0 a 3)
    }

    [System.Serializable]
    public class AnswerSummary
    {
        public string questionText;
        public string userAnswer;
        public string correctAnswer;
    }

    // Riferimento al PlayerMovement per controllare il movimento del giocatore
    public PlayerMovement playerMovement;
    private List<AnswerSummary> answerSummaries = new List<AnswerSummary>();

    public List<Question> questions; // Lista delle domande
    private int currentQuestionIndex = 0;
    private int correctAnswersCount = 0;
    private int incorrectAnswersCount = 0;

    public Text questionText; // Riferimento al componente UI per il testo della domanda
    public Button[] answerButtons; // Array dei pulsanti di risposta
    public GameObject resultsPanel; // Pannello per visualizzare i risultati finali
    public Text resultsText; // Testo per visualizzare i risultati finali

    // Aggiungi questi nuovi riferimenti
    public GameObject questionPanel; // Il pannello che contiene le domande e le risposte

    void Start()
    {
        resultsPanel.SetActive(false); // Nascondi i risultati all'inizio
        questionPanel.SetActive(false); // Nascondi il pannello delle domande
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
        questionPanel.SetActive(true); // Mostra il pannello delle domande
        LoadQuestion(); // Inizia dal primo quiz
    }

    void LoadQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            // Imposta il testo della domanda
            questionText.text = questions[currentQuestionIndex].questionText;

            // Imposta il testo per ogni pulsante di risposta
            for (int i = 0; i < answerButtons.Length; i++)
            {
                Text answerText = answerButtons[i].GetComponentInChildren<Text>();
                if (answerText != null)
                {
                    answerText.text = questions[currentQuestionIndex].answers[i];
                }

                // Rimuovi tutti i listener precedenti per evitare duplicati
                answerButtons[i].onClick.RemoveAllListeners();

                // Aggiungi un listener che chiama CheckAnswer quando viene cliccato
                int answerIndex = i; // Variabile locale per evitare problemi con le closures
                answerButtons[i].onClick.AddListener(() => CheckAnswer(answerIndex));
            }
        }
        else
        {
            ShowResults(); // Non ci sono più domande, mostra i risultati
        }
    }

    void CheckAnswer(int selectedAnswerIndex)
    {
        // Assicurati che non si selezioni una risposta dopo che il quiz è finito
        if (currentQuestionIndex >= questions.Count)
        {
            return; // Evita di rispondere dopo aver raggiunto la fine del quiz
        }

        Debug.Log("Selected answer: " + selectedAnswerIndex);

        // Salva la domanda, la risposta data dall'utente e quella corretta
        var summary = new AnswerSummary
        {
            questionText = questions[currentQuestionIndex].questionText,
            userAnswer = questions[currentQuestionIndex].answers[selectedAnswerIndex],
            correctAnswer = questions[currentQuestionIndex].answers[questions[currentQuestionIndex].correctAnswerIndex]
        };
        answerSummaries.Add(summary);

        // Controlla se la risposta è corretta
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
        // Nascondi il pannello delle domande (questionPanel) e i bottoni delle risposte
        questionPanel.SetActive(false); // Nasconde il pannello delle domande
        foreach (Button btn in answerButtons) // Nasconde ogni bottone delle risposte
        {
            btn.gameObject.SetActive(false);
        }

        // Mostra il pannello dei risultati
        resultsPanel.SetActive(true);

        // Crea un testo formattato per ogni domanda e risposta
        string resultMessage = "<b>Risultati del Quiz</b>\n\n";
        foreach (var summary in answerSummaries)
        {
            resultMessage += "<b>Domanda:</b> " + summary.questionText + "\n";
            resultMessage += "<i>Risposta dell'utente:</i> " + summary.userAnswer + "\n";
            resultMessage += "<i>Risposta corretta:</i> " + summary.correctAnswer + "\n\n";
        }

        // Aggiungi il conteggio delle risposte corrette e sbagliate alla fine
        resultMessage += "<color=green>Risposte corrette: " + correctAnswersCount + "</color>\n";
        resultMessage += "<color=red>Risposte errate: " + incorrectAnswersCount + "</color>\n\n";

        // Aggiungi un'istruzione per chiudere il pannello dei risultati
        resultMessage += "<i>Premi Invio per chiudere questo pannello e continuare il gioco.</i>";

        resultsText.text = resultMessage; // Assegna il testo formattato al componente Text
    }

    public void RestartQuiz()
    {
        currentQuestionIndex = 0;
        correctAnswersCount = 0;
        incorrectAnswersCount = 0;
        answerSummaries.Clear(); // Reset delle risposte salvate

        // Mostra di nuovo il pannello delle domande e nascondi il pannello dei risultati
        questionPanel.SetActive(true);
        resultsPanel.SetActive(false);

        // Avvia una nuova partita
        LoadQuestion();
    }

    void Update()
    {
        // Controlla se il pannello dei risultati è attivo e l'utente preme Invio
        if (resultsPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            // Chiudi il pannello dei risultati
            resultsPanel.SetActive(false);

            // Non riattivare il pannello delle domande né i bottoni delle risposte
            questionPanel.SetActive(false);
            foreach (Button btn in answerButtons)
            {
                btn.gameObject.SetActive(false);
            }

            // Permetti al giocatore di muoversi di nuovo
            if (playerMovement != null)
            {
                playerMovement.isQuizActive = false;
            }
        }
    }
}
