using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject finishBox;
    public bool playerInRange;
    public VectorValue startingPositionDynamic;  // Oggetto che contiene la posizione dinamica del giocatore
    public VectorValue startingPositionPreviousScene;  // Oggetto per memorizzare la posizione della scena precedente

    // Variabile per disabilitare temporaneamente il movimento del player
    private PlayerMovement playerMovementScript;

    // Start is called before the first frame update
    void Start()
    {
        // Assicura che la schermata sia nascosta all'inizio
        finishBox.SetActive(false);
    }

    void Update()
    {
        // Controlla se il pannello è visibile e il player preme "invio"
        if (playerInRange && finishBox.activeInHierarchy && Input.GetKeyDown(KeyCode.Return))
        {
            ClosePanelAndLoadScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            finishBox.SetActive(true); // Attiva la schermata quando il player entra
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            finishBox.SetActive(false); // Disattiva la schermata quando il player esce
        }
    }

    // Funzione per chiudere il pannello e caricare la scena
    void ClosePanelAndLoadScene()
    {
        finishBox.SetActive(false);  // Chiude il pannello
        startingPositionDynamic.initialValue = startingPositionPreviousScene.initialValue;

        // Ottieni lo script PlayerMovement per disabilitare temporaneamente il movimento
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerMovementScript = player.GetComponent<PlayerMovement>();

            // Disabilita temporaneamente il movimento del player
            if (playerMovementScript != null)
            {
                playerMovementScript.PreventPositionUpdate(); // Disabilita la gestione della posizione
            }

            // Ripristina la posizione del giocatore dalla variabile startingPositionPreviousScene
            player.transform.position = startingPositionPreviousScene.initialValue;
            Debug.Log("Posizione del giocatore ripristinata: " + startingPositionPreviousScene.initialValue);
        }

        // Carica la scena della classe
        SceneManager.LoadScene("LeftClassInterior");
    }
}


