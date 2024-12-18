using UnityEngine;
using TMPro;

public class ComponentHoverIndicatorTextMeshPro : MonoBehaviour
{
    public GameObject canvasBubble;
    public GameObject hoverBubble;
    public string componentName;
    private TextMeshProUGUI bubbleText;

    void Start()
    {
        bubbleText = hoverBubble.GetComponentInChildren<TextMeshProUGUI>();
        if (bubbleText != null)
        {
            bubbleText.text = componentName;
        }

        canvasBubble.SetActive(false);
        hoverBubble.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canvasBubble.SetActive(true);
            hoverBubble.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hoverBubble.SetActive(false);
            canvasBubble.SetActive(false);
        }
    }
}
