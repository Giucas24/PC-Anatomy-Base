using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroSequence : MonoBehaviour
{
    public TMP_Text dialogText; // Riferimento al dialogText nella dialog box
    public GameObject answers; // Contenitore dei bottoni delle componenti
    public Button[] componentButtons; // Array di bottoni delle componenti (dentro Answers)
    public string[] introductionTexts; // Array di stringhe per i testi introduttivi
    public string[] componentDescriptions; // Array di descrizioni per ogni componente
    public string finalIntroText; // L'ultima frase introduttiva

    private int currentIndex = 0; // Contatore per tracciare il dialogo corrente
    private bool showingComponents = false; // Indica se siamo nella fase dei componenti
    private bool showingFinalIntro = false; // Indica se stiamo mostrando la frase finale

    public QuestionManager questionManager;

    // Aggiunta della variabile per segnare se il quiz è completato
    private bool quizCompleted = false;

    void Start()
    {
        // Assicuriamoci che i bottoni siano inizialmente nascosti
        foreach (Button button in componentButtons)
        {
            button.gameObject.SetActive(false);
        }

        ShowNextText(); // Mostriamo il primo testo
    }

    void Update()
    {
        // Se il quiz è stato completato, non permettiamo più di avanzare nei testi introduttivi
        if (quizCompleted)
            return;

        // Controlliamo se il giocatore preme la barra spaziatrice
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (showingFinalIntro)
            {
                StartQuiz(); // Passa al quiz
            }
            else if (!showingComponents)
            {
                ShowNextText(); // Avanziamo nei testi introduttivi
            }
            else
            {
                ShowNextComponent(); // Avanziamo nella fase delle componenti
            }
        }
    }

    public void ShowNextText()
    {
        if (currentIndex < introductionTexts.Length)
        {
            dialogText.text = introductionTexts[currentIndex]; // Mostriamo il testo corrente
            currentIndex++;
        }
        else
        {
            showingComponents = true; // Passiamo alla fase delle componenti
            currentIndex = 0;
            ShowNextComponent();
        }
    }

    public void ShowNextComponent()
    {
        if (currentIndex < componentDescriptions.Length)
        {
            dialogText.text = componentDescriptions[currentIndex]; // Mostriamo la descrizione
            componentButtons[currentIndex].gameObject.SetActive(true); // Mostriamo il bottone corrispondente
            currentIndex++;
        }
        else
        {
            ShowFinalIntro(); // Mostra l'ultima frase introduttiva
        }
    }

    void ShowFinalIntro()
    {
        showingComponents = false; // Fine della fase componenti
        showingFinalIntro = true;  // Inizia la frase finale
        dialogText.text = finalIntroText;
    }

    void StartQuiz()
    {
        dialogText.text = "Pronto per iniziare il quiz!"; // Messaggio opzionale
        questionManager.StartGame(); // Avvia il gioco
    }

    // Metodo per segnare il completamento del quiz
    public void CompleteQuiz()
    {
        quizCompleted = true; // Imposta il flag per segnare che il quiz è completato
    }
}
