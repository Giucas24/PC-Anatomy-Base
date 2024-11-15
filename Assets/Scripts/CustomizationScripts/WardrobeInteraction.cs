using UnityEngine;

public class WardrobeInteraction : MonoBehaviour
{
    public GameObject wardrobeUI; // Il Canvas che mostra le opzioni di skin
    private bool isPlayerNearby = false; // Indica se il giocatore è vicino all'armadio

    void Update()
    {
        // Controlla se il giocatore preme il tasto "Spazio" ed è vicino all'armadio
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Space))
        {
            ToggleWardrobeUI();
        }
    }

    private void ToggleWardrobeUI()
    {
        if (wardrobeUI != null)
        {
            // Attiva o disattiva il Canvas
            bool isActive = wardrobeUI.activeSelf;
            wardrobeUI.SetActive(!isActive);
        }
        else
        {
            Debug.LogError("Il riferimento al Canvas dell'armadio non è impostato!");
        }
    }

    // Quando il giocatore entra nel trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Assicurati che il Player abbia il tag "Player"
        {
            isPlayerNearby = true;
        }
    }

    // Quando il giocatore esce dal trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}

