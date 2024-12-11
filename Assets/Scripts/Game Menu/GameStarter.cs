using UnityEngine;
using UnityEngine.SceneManagement; // Importa per caricare le scene
using UnityEngine.UI; // Importa per lavorare con i bottoni

public class GameStarter : MonoBehaviour
{
    public Button playButton;  // Bottone per "Gioca"
    public Button exitButton;  // Bottone per "Esci"

    // Nome della scena di gioco che vogliamo caricare
    public string gameSceneName;  // Modifica con il nome della scena che vuoi caricare
    public string exitSceneName;  // Modifica con la scena principale, se necessario

    void Start()
    {
        // Assicurati che i bottoni siano assegnati correttamente
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
    }

    // Metodo che viene chiamato quando il bottone "Gioca" viene premuto
    void OnPlayButtonClicked()
    {
        // Carica la scena di gioco
        SceneManager.LoadScene(gameSceneName);
    }

    // Metodo che viene chiamato quando il bottone "Esci" viene premuto
    void OnExitButtonClicked()
    {
        // Se siamo nel gioco (build), chiudi l'applicazione
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Ferma il gioco nell'Editor
#else
        Application.Quit();  // Chiudi l'applicazione
#endif
    }
}
