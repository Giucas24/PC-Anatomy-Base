using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionPlayerInteraction : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition; // La nuova posizione del player nella scena successiva
    public VectorValue startingPositionDynamic; // Solo la posizione dinamica viene aggiornata
    private bool playerInTrigger = false; // Indica se il player è nel trigger

    void Update()
    {
        // Controlla se il player è nel trigger e se è stato premuto il tasto Spazio
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            // Aggiorna il valore dinamico
            startingPositionDynamic.initialValue = playerPosition;

            // Debug per verificare l'ordine
            Debug.Log("Aggiornato startingPositionDynamic a: " + startingPositionDynamic.initialValue);

            // Carica la nuova scena
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            // Imposta il flag per indicare che il player è nel trigger
            playerInTrigger = true;
            Debug.Log("Player è entrato nel trigger. Premi Spazio per cambiare scena.");
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            // Resetta il flag quando il player esce dal trigger
            playerInTrigger = false;
            Debug.Log("Player è uscito dal trigger.");
        }
    }
}
