using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionPlayerTransition : MonoBehaviour
{
    public string sceneToLoad;  // La scena da caricare
    public Vector2 playerPosition; // La posizione del giocatore nella nuova scena
    public VectorValue startingPositionDynamic;  // Posizione dinamica che viene aggiornata prima del cambio scena
    public VectorValue startingPositionPreviousScene;  // Nuova variabile per memorizzare la posizione della scena precedente
    private bool playerInTrigger = false;  // Indica se il giocatore è nel trigger

    void Update()
    {
        // Controlla se il player è nel trigger e se è stato premuto il tasto Spazio
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Space))
        {

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                startingPositionPreviousScene.initialValue = player.transform.position;
                Debug.Log("Posizione salvata per la scena precedente: " + startingPositionPreviousScene.initialValue);

                // Salva la posizione dinamica nella scena successiva
                startingPositionDynamic.initialValue = playerPosition;
                Debug.Log("Posizione salvata per la nuova scena: " + startingPositionDynamic.initialValue);

                // Carica la nuova scena
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("Player non trovato!");
            }
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
