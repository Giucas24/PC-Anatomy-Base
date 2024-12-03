using UnityEngine;
using UnityEngine.SceneManagement; // Necessario per caricare scene

public class SceneLoader : MonoBehaviour
{
    // Nome della scena da caricare
    [SerializeField] private string sceneName;

    private bool playerInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Controlla se il player è entrato nel Box Collider
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Controlla se il player è uscito dal Box Collider
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    private void Update()
    {
        // Se il player è all'interno del Box Collider e preme spazio, carica la scena
        if (playerInside && Input.GetKeyDown(KeyCode.Space))
        {
            LoadScene();
        }
    }

    private void LoadScene()
    {
        // Salva la scena corrente nei PlayerPrefs prima di caricare la nuova
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("PreviousScene", currentScene);

        // Carica la scena specificata
        SceneManager.LoadScene(sceneName);
    }
}
