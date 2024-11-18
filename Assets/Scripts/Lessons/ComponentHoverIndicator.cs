using UnityEngine;
using UnityEngine.UI; // Per il componente Text

public class ComponentHover : MonoBehaviour
{
    public GameObject canvasBubble; // Il Canvas Bubble
    public GameObject hoverBubble; // La nuvoletta
    public string componentName; // Nome del componente da mostrare
    private Text bubbleText; // Il testo all'interno della nuvoletta

    void Start()
    {
        // Trova il componente Text all'interno della nuvoletta
        bubbleText = hoverBubble.GetComponentInChildren<Text>();
        if (bubbleText != null)
        {
            bubbleText.text = componentName; // Imposta il testo del componente
        }

        // Nascondi la nuvoletta e il Canvas all'inizio
        canvasBubble.SetActive(false);
        hoverBubble.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Attiva il Canvas e la nuvoletta
            canvasBubble.SetActive(true);
            hoverBubble.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Nasconde la nuvoletta e il Canvas quando il player esce dal trigger
            hoverBubble.SetActive(false);
            canvasBubble.SetActive(false);
        }
    }
}
