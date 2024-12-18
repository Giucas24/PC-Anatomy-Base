using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialBox;
    public TextMeshProUGUI tutorialText;
    private bool isVisible = false;
    public PlayerMovement playerMovement;
    private static TutorialManager instance;

    public List<string> scenesWithoutTutorial;
    private bool tutorialShown = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
            Debug.LogWarning("TutorialBox not assigned in Inspector");
        }

        SetCanvasCamera();
    }

    void Update()
    {
        if (!scenesWithoutTutorial.Contains(SceneManager.GetActiveScene().name) && Input.GetKeyDown(KeyCode.T))
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

        // Show tutorial at the start of the game
        if (scene.name == "HouseInterior" && !tutorialShown)
        {
            isVisible = true;
            if (tutorialBox != null)
            {
                tutorialBox.SetActive(true);
            }
            ShowInitialTutorial();
            playerMovement.isTutorialActive = true;

            tutorialShown = true;
        }
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
                Debug.LogWarning("MainCamera not found");
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
            Debug.LogWarning("MainCamera not found during the centering");
        }
    }

    public void ShowTutorial(string message)
    {
        if (tutorialText != null)
        {
            tutorialText.text = message;
        }
    }

    void ShowInitialTutorial()
    {
        string tutorialMessage = "Benvenuto nel gioco! Ecco come puoi interagire:\n\n" +
            "1. Premi <b>Spazio</b> per interagire con l'ambiente.\n" +
            "2. Premi <b>Invio</b> per chiudere i pannelli dei risultati\n" +
            "3. Premi <b>T</b> per aprire e chiudere questo tutorial.\n\n" +
            "Buon divertimento!";

        ShowTutorial(tutorialMessage);
    }
}
