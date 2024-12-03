using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition; // La nuova posizione del player nella scena successiva
    public VectorValue startingPositionDynamic; // Solo la posizione dinamica viene aggiornata

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            // Aggiorna il valore dinamico
            startingPositionDynamic.initialValue = playerPosition;

            // Debug per verificare l'ordine
            Debug.Log("Aggiornato startingPositionDynamic a: " + startingPositionDynamic.initialValue);

            // Carica la nuova scena
            SceneManager.LoadScene(sceneToLoad);
        }
    }

}
