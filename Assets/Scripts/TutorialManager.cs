using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialBox; // Riferimento al TutorialBox
    public Text tutorialText; // Riferimento al testo del tutorial
    private bool isVisible = false; // Stato iniziale del TutorialBox
    public PlayerMovement playerMovement; // Riferimento allo script PlayerMovement
    private static TutorialManager instance; // Singleton

    void Awake()
    {
        // Implementazione del pattern Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Rendi persistente il Canvas e i suoi figli
        }
        else
        {
            Destroy(gameObject); // Elimina i duplicati
        }
    }

    void Start()
    {
        // Assicurati che il TutorialBox sia nascosto all'inizio
        if (tutorialBox != null)
        {
            tutorialBox.SetActive(isVisible);
        }
        else
        {
            Debug.LogWarning("TutorialBox non assegnato nel pannello Inspector!");
        }

        // Mostra il tutorial iniziale
        ShowInitialTutorial();

        // Imposta la camera corretta al primo avvio (se è in modalità WorldSpace)
        SetCanvasCamera();
    }

    void Update()
    {
        // Controlla se il tasto T viene premuto
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTutorialBox();
        }
    }

    void OnEnable()
    {
        // Rileva quando una scena è stata caricata
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Rimuovi il listener per evitare problemi di memoria
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Funzione chiamata ogni volta che una scena viene caricata
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Imposta la camera ogni volta che una scena viene caricata
        SetCanvasCamera();
    }

    // Funzione per impostare correttamente la camera nel Canvas se in World Space
    void SetCanvasCamera()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
        {
            Camera mainCamera = Camera.main; // Ottieni la Camera principale
            if (mainCamera != null)
            {
                canvas.worldCamera = mainCamera; // Imposta la camera del Canvas
            }
            else
            {
                Debug.LogWarning("Non è stata trovata la Camera principale!");
            }
        }
    }

    // Funzione per mostrare/nascondere il TutorialBox
    void ToggleTutorialBox()
    {
        if (tutorialBox != null)
        {
            isVisible = !isVisible;
            tutorialBox.SetActive(isVisible);

            // Se il TutorialBox è visibile, centriamo il box sulla camera
            if (isVisible)
            {
                CenterTutorialBoxOnCamera();
            }

            // Abilita o disabilita il movimento del giocatore
            playerMovement.isTutorialActive = isVisible; // Disabilita il movimento se il tutorial è attivo
        }
    }

    // Funzione per centrare il TutorialBox sulla camera
    void CenterTutorialBoxOnCamera()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            // Ottieni la posizione della camera nel mondo
            Vector3 cameraPosition = mainCamera.transform.position;

            // Imposta la posizione del TutorialBox al centro della camera
            // La z è mantenuta costante (di solito 0) per non alterare la profondità
            tutorialBox.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, tutorialBox.transform.position.z);
        }
        else
        {
            Debug.LogWarning("Camera principale non trovata durante il centraggio del TutorialBox");
        }
    }

    // Funzione per mostrare il tutorial
    public void ShowTutorial(string message)
    {
        if (tutorialText != null)
        {
            tutorialText.text = message;
        }
    }

    // Funzione per mostrare il tutorial iniziale con i tasti del gioco
    void ShowInitialTutorial()
    {
        string tutorialMessage = "Benvenuto nel gioco! Ecco come puoi interagire:\n\n" +
            "1. Premi <b>Spazio</b> per interagire con l'ambiente.\n" +
            "2. Premi <b>Invio</b> per chiudere il pannello del quiz a risposta multipla.\n" +
            "3. Premi <b>T</b> per aprire e chiudere questo tutorial.\n\n" +
            "Buon divertimento!";

        ShowTutorial(tutorialMessage);
    }
}
