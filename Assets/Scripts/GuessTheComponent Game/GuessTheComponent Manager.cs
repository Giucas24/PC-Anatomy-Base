using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;

public class GuessTheComponentManager : MonoBehaviour
{
    public GameObject[] componentObjects; // Gli oggetti che contengono le immagini delle componenti
    private List<int> usedIndices = new List<int>(); // Lista degli indici già usati
    private int currentComponentIndex;
    private string currentWordToGuess;
    private float timer = 20f; // Tempo per ogni round (20 secondi)

    // Riferimenti per l'UI
    public TMP_InputField answerInput;  // Campo di input per la risposta
    public Button submitButton;         // Pulsante per inviare la risposta
    public TMP_Text feedbackText;       // Testo per il feedback (corretto o sbagliato)
    public GameObject wordContainer;   // Contenitore dei trattini per la parola
    public TextMeshProUGUI timerText;  // Testo del timer (da collegare nell'Inspector)

    // Variabili per gestire la rivelazione delle lettere
    private float revealLetterTimer = 5f; // Tempo per rivelare ogni lettera (fisso)
    private int currentLetterIndex = 0;  // Indice della lettera che dobbiamo rivelare
    private int maxLettersToReveal = 3;  // Numero massimo di lettere da rivelare per round

    private bool isRoundComplete = false; // Flag per sapere se il round è completo
    private bool isAnswerCorrect = false; // Flag per sapere se la risposta è corretta

    // Nuove variabili per rendere visibili gli elementi UI quando il gioco inizia
    public GameObject dialogBox; // Il dialog box da disabilitare
    public GameObject[] gameUIElements; // Gli oggetti da abilitare (AnswerInput, TimerText, etc.)

    private bool isGameActive = false; // Indica se il gioco è in corso

    private List<int> revealedIndices = new List<int>(); // Indici delle lettere già rivelate




    public void StartGame()
    {
        dialogBox.SetActive(false);

        foreach (var element in gameUIElements)
        {
            element.SetActive(true);
        }

        foreach (var component in componentObjects)
        {
            component.SetActive(false);
        }

        submitButton.onClick.AddListener(SubmitAnswer);

        isGameActive = true; // Il gioco è ora attivo
        StartRound();
    }


    void StartRound()
    {
        isGameActive = true; // Il gioco è attivo
        foreach (var component in componentObjects)
        {
            component.SetActive(false);
        }

        if (usedIndices.Count == componentObjects.Length)
        {
            usedIndices.Clear();
        }

        do
        {
            currentComponentIndex = Random.Range(0, componentObjects.Length);
        } while (usedIndices.Contains(currentComponentIndex));

        usedIndices.Add(currentComponentIndex);
        componentObjects[currentComponentIndex].SetActive(true);

        currentWordToGuess = componentObjects[currentComponentIndex].name;

        feedbackText.text = "";
        answerInput.text = "";

        DisplayWordWithDashes(currentWordToGuess);

        timer = 20f;
        currentLetterIndex = 0;

        // Resetta la lista degli indici rivelati
        revealedIndices.Clear();

        answerInput.Select();
        answerInput.ActivateInputField();

        isRoundComplete = false;
        isAnswerCorrect = false;
    }



    void Update()
    {
        // Esegui solo se il dialogo è terminato
        if (IntroDialogManager.isDialogActive) return;

        // Se il round è completo, non fare nulla
        if (isRoundComplete) return;

        // Timer del round
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timer).ToString();
        }
        else
        {
            timerText.text = "Tempo scaduto!";
            isRoundComplete = true;
            StartCoroutine(WaitForNextRound());
        }

        // Timer per rivelare lettere
        if (revealLetterTimer > 0f)
        {
            revealLetterTimer -= Time.deltaTime;
        }
        else if (currentLetterIndex < maxLettersToReveal && currentLetterIndex < currentWordToGuess.Length)
        {
            RevealNextLetter();
            revealLetterTimer = 5f;
        }

        // Rilevamento del tasto "Invio" per confermare la risposta
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SubmitAnswer();
        }

        // Blocca il tasto "Spazio" durante il gioco
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Spazio premuto, ma il gioco è attivo e non succede nulla.");
        }
    }






    void SubmitAnswer()
    {
        // Rimuove eventuali spazi iniziali e finali dalla risposta dell'utente
        string trimmedAnswer = answerInput.text.Trim();

        // Verifica se la risposta è corretta
        if (trimmedAnswer.ToLower() == currentWordToGuess.ToLower())
        {
            feedbackText.text = "Corretto!";
            RevealFullWord(); // Mostra la parola completa se la risposta è corretta
            isAnswerCorrect = true;  // La risposta è corretta
            isRoundComplete = true;  // Completa il round

            // Avvia la transizione al prossimo round
            StartCoroutine(WaitForNextRound());
        }
        else
        {
            feedbackText.text = "Sbagliato, riprova!";
        }

        // Resetta il campo di input per una nuova risposta
        answerInput.text = ""; // Azzera il testo corrente

        // Mantieni il focus sull'input field
        answerInput.Select();
        answerInput.ActivateInputField(); // Riattiva l'input field (assicurati che il cursore sia visibile)
    }


    // Funzione che termina il round e avvia il prossimo
    IEnumerator WaitForNextRound()
    {
        isGameActive = false; // Il gioco non è più attivo
        yield return new WaitForSeconds(2f);

        foreach (var component in componentObjects)
        {
            component.SetActive(false);
        }

        StartRound();
    }


    // Funzione per visualizzare la parola con trattini
    void DisplayWordWithDashes(string word)
    {
        // Rimuove eventuali trattini vecchi
        foreach (Transform child in wordContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Aggiungi un "Dash" per ogni lettera della parola
        foreach (char letter in word)
        {
            GameObject dash = new GameObject("Dash");
            dash.transform.SetParent(wordContainer.transform);  // Imposta WordContainer come genitore

            TextMeshProUGUI dashText = dash.AddComponent<TextMeshProUGUI>();
            dashText.text = "-";  // Ogni trattino rappresenta una lettera
            dashText.fontSize = 32; // Imposta la dimensione del font

            // Puoi anche aggiungere altre proprietà di styling, come il colore
            dashText.color = Color.black;

            // Imposta il layout (se stai usando un HorizontalLayoutGroup, imposta larghezza e altezza)
            RectTransform dashRectTransform = dash.GetComponent<RectTransform>();
            dashRectTransform.sizeDelta = new Vector2(40, 40); // Imposta le dimensioni (modifica come desiderato)
        }

        // Assicurati che WordContainer sia visibile
        wordContainer.SetActive(true);
    }

    // Funzione che rivela la prossima lettera
    void RevealNextLetter()
    {
        // Controlla se ci sono ancora lettere non rivelate
        if (revealedIndices.Count < currentWordToGuess.Length)
        {
            int randomIndex;

            // Trova un indice non ancora rivelato
            do
            {
                randomIndex = Random.Range(0, currentWordToGuess.Length);
            } while (revealedIndices.Contains(randomIndex));

            // Aggiungi l'indice alla lista di quelli rivelati
            revealedIndices.Add(randomIndex);

            // Trova l'oggetto dash corrispondente
            Transform dashTransform = wordContainer.transform.GetChild(randomIndex);

            // Modifica il testo del Dash per mostrare la lettera corrispondente
            TextMeshProUGUI dashText = dashTransform.GetComponent<TextMeshProUGUI>();
            dashText.text = currentWordToGuess[randomIndex].ToString();
        }
    }


    // Funzione per rivelare tutta la parola
    void RevealFullWord()
    {
        // Riveliamo tutte le lettere della parola
        foreach (Transform child in wordContainer.transform)
        {
            TextMeshProUGUI dashText = child.GetComponent<TextMeshProUGUI>();
            int index = child.GetSiblingIndex();
            dashText.text = currentWordToGuess[index].ToString();
        }
    }
}
