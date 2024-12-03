using UnityEngine;
using TMPro; // Aggiungi la libreria TextMeshPro
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialBox; // Riferimento al TutorialBox
    public TextMeshProUGUI tutorialText; // Modificato da Text a TextMeshProUGUI
    private bool isVisible = false; // Stato iniziale del TutorialBox
    public PlayerMovement playerMovement; // Riferimento allo script PlayerMovement
    private static TutorialManager instance; // Singleton

    void Awake()
    {
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
        if (tutorialBox != null)
        {
            tutorialBox.SetActive(isVisible);
        }
        else
        {
            Debug.LogWarning("TutorialBox non assegnato nel pannello Inspector!");
        }

        ShowInitialTutorial();
        SetCanvasCamera();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTutorialBox();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetCanvasCamera();
    }

    void SetCanvasCamera()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                canvas.worldCamera = mainCamera;
            }
            else
            {
                Debug.LogWarning("Non è stata trovata la Camera principale!");
            }
        }
    }

    void ToggleTutorialBox()
    {
        if (tutorialBox != null)
        {
            isVisible = !isVisible;
            tutorialBox.SetActive(isVisible);

            if (isVisible)
            {
                CenterTutorialBoxOnCamera();
            }

            playerMovement.isTutorialActive = isVisible;
        }
    }

    void CenterTutorialBoxOnCamera()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Vector3 cameraPosition = mainCamera.transform.position;
            tutorialBox.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, tutorialBox.transform.position.z);
        }
        else
        {
            Debug.LogWarning("Camera principale non trovata durante il centraggio del TutorialBox");
        }
    }

    public void ShowTutorial(string message)
    {
        if (tutorialText != null)
        {
            tutorialText.text = message; // Il comportamento rimane lo stesso
        }
    }

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
