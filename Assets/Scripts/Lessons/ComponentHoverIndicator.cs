using UnityEngine;
using UnityEngine.UI;

public class ComponentHover : MonoBehaviour
{
    public GameObject canvasBubble;
    public GameObject hoverBubble;
    public string componentName;
    private Text bubbleText;

    void Start()
    {
        bubbleText = hoverBubble.GetComponentInChildren<Text>();
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
