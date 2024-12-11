using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class GuessTheComponentManager : MonoBehaviour
{
    public TMP_FontAsset orangeKidFont;
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

    private int correctAnswers = 0;  // Numero di risposte corrette
    private int totalQuestions = 0; // Contatore delle domande affrontate
    private const int maxQuestions = 10; // Numero massimo di domande

    public TextMeshProUGUI dialogText; // Testo del dialogBox, da usare per il riepilogo
    private bool isGameSummaryActive = false; // Indica se il riepilogo è attivo

    public VectorValue startingPositionDynamic;
    public VectorValue startingPositionPreviousScene;

    private List<int> randomizedIndices; // Lista degli indici randomizzati






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

        // Mescola gli indici all'inizio del gioco
        randomizedIndices = new List<int>();
        for (int i = 0; i < componentObjects.Length; i++)
        {
            randomizedIndices.Add(i);
        }
        ShuffleList(randomizedIndices); // Mescola la lista

        submitButton.onClick.AddListener(SubmitAnswer);

        isGameActive = true; // Il gioco è ora attivo
        StartRound();
    }

    void ShuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }


    void StartRound()
    {
        if (totalQuestions >= maxQuestions || totalQuestions >= randomizedIndices.Count)
        {
            EndGame(); // Termina il gioco e mostra il riepilogo
            return;
        }

        totalQuestions++; // Incrementa il contatore delle domande

        isGameActive = true; // Il gioco è attivo
        foreach (var component in componentObjects)
        {
            component.SetActive(false);
        }

        // Usa il prossimo indice nella lista randomizzata
        currentComponentIndex = randomizedIndices[totalQuestions - 1];
        componentObjects[currentComponentIndex].SetActive(true);

        currentWordToGuess = componentObjects[currentComponentIndex].name;

        feedbackText.text = "";
        answerInput.text = "";

        DisplayWordWithDashes(currentWordToGuess);

        timer = 20f;
        currentLetterIndex = 0;

        revealedIndices.Clear();

        answerInput.Select();
        answerInput.ActivateInputField();

        isRoundComplete = false;
        isAnswerCorrect = false;
    }



    void Update()
    {
        // Esegui solo se il dialogo iniziale è terminato
        if (IntroDialogManager.isDialogActive) return;

        if (isGameSummaryActive && Input.GetKeyDown(KeyCode.Return))
        {
            // Torna alla scena "LeftClassInterior"
            SceneManager.LoadScene("LeftClassInterior");
        }

        if (!isGameActive || isRoundComplete) return;

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

        if (revealLetterTimer > 0f)
        {
            revealLetterTimer -= Time.deltaTime;
        }
        else if (currentLetterIndex < maxLettersToReveal && currentLetterIndex < currentWordToGuess.Length)
        {
            RevealNextLetter();
            revealLetterTimer = 5f;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SubmitAnswer();
        }
    }






    void SubmitAnswer()
    {
        string trimmedAnswer = answerInput.text.Trim();

        if (trimmedAnswer.ToLower() == currentWordToGuess.ToLower())
        {
            correctAnswers++; // Incrementa il punteggio se corretto
            feedbackText.text = "Corretto!";
            RevealFullWord();
            isAnswerCorrect = true;
            isRoundComplete = true;

            StartCoroutine(WaitForNextRound());
        }
        else
        {
            feedbackText.text = "Sbagliato, riprova!";
        }

        answerInput.text = "";
        answerInput.Select();
        answerInput.ActivateInputField();
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
            dashText.font = orangeKidFont;

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

    void EndGame()
    {
        isGameActive = false;
        isGameSummaryActive = true; // Attiva la fase di riepilogo

        // Controlla se il giocatore ha risposto correttamente ad almeno 8 domande
        if (correctAnswers >= 8)
        {
            if (SkinManager.Instance != null)
            {
                Debug.Log("Chiamata a SkinManager.Instance.UnlockSkin()");
                SkinManager.Instance.UnlockSkin(4); // Sblocca la skin con indice 5
            }
            else
            {
                Debug.LogWarning("SkinManager.Instance è null!");
            }
        }

        // Aggiorna la posizione del giocatore
        startingPositionDynamic.initialValue = startingPositionPreviousScene.initialValue;

        // Ottieni il giocatore e disabilita temporaneamente il movimento
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerMovement playerMovementScript = player.GetComponent<PlayerMovement>();

            if (playerMovementScript != null)
            {
                playerMovementScript.PreventPositionUpdate(); // Disabilita la gestione della posizione
            }

            // Ripristina la posizione del giocatore
            player.transform.position = startingPositionPreviousScene.initialValue;
            Debug.Log("Posizione del giocatore ripristinata: " + startingPositionPreviousScene.initialValue);
        }

        // Mostra il dialogBox con il riepilogo
        dialogBox.SetActive(true);

        if (correctAnswers >= maxQuestions / 2) // Almeno metà delle risposte corrette
        {
            dialogText.text = $"Congratulazioni! Hai risposto correttamente a {correctAnswers} su {maxQuestions} domande. Premi <b>Invio</b> per tornare alla classe.";
        }
        else // Meno della metà delle risposte corrette
        {
            dialogText.text = $"Peccato! Hai risposto correttamente a {correctAnswers} su {maxQuestions} domande. Premi <b>Invio</b> per tornare alla classe.";
        }

        // Disattiva gli elementi di gioco
        foreach (var element in gameUIElements)
        {
            element.SetActive(false);
        }

        // Disattiva tutte le componentObjects
        foreach (var component in componentObjects)
        {
            component.SetActive(false);
        }
    }


}
