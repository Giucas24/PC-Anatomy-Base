using UnityEngine;
using UnityEngine.SceneManagement; // Importa per gestire gli eventi di cambio scena

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public Vector2 maxPosition;
    public Vector2 minPosition;

    void Start()
    {
        if (target == null)
        {
            FindPlayer(); // Trova il player se non è stato assegnato
        }

        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }

    void OnEnable()
    {
        // Registra l'evento per il cambio di scena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Deli­ga la registrazione quando l'oggetto viene distrutto
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer();  // Riassegna il target quando la scena è cambiata
    }

    private void FindPlayer()
    {
        // Cerca un oggetto con il tag "Player"
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            target = player.transform;
            Debug.Log("Player assegnato come target della camera.");
        }
        else
        {
            Debug.LogError("Player non trovato! Assicurati che il Player abbia il tag 'Player'.");
        }
    }

}
