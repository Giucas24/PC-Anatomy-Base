using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class CanvasCameraUpdater : MonoBehaviour
{
    private Canvas canvas; // Riferimento al canvas

    void Awake()
    {
        // Assicura che il canvas sia persistente tra le scene
        DontDestroyOnLoad(gameObject);

        // Ottieni il componente Canvas
        canvas = GetComponent<Canvas>();

        // Prova a impostare immediatamente la camera
        SetCanvasCamera();
    }

    void OnEnable()
    {
        // Registra l'evento quando una scena viene caricata
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Deregistra l'evento per evitare problemi di memoria
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Riprova a trovare la camera ogni volta che cambia scena
        SetCanvasCamera();
    }

    private void SetCanvasCamera()
    {
        // Trova la Main Camera nella scena
        Camera mainCamera = Camera.main;

        if (mainCamera != null && canvas != null)
        {
            // Imposta la Main Camera come camera del canvas
            canvas.worldCamera = mainCamera;
            Debug.Log("Camera assegnata al Canvas: " + mainCamera.name);
        }
        else
        {
            Debug.LogWarning("Main Camera non trovata o Canvas non disponibile!");
        }
    }
}
